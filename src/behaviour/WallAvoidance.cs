using Godot;

namespace Behaviours
{
    public class WallAvoidance : Behaviour
    {
        private Vision vision;
        private Node2D parent;
        private float avoidDistance; // Distance to start avoiding

        private Vector2? lastClosestWorldPoint = null;

        public WallAvoidance(string name, Vision vision, Node2D parent)
        {
            this.weight = PersistentParameter.ParameterRegistry.GetFloatParameter($"{name}.WallAvoidanceWeight", 1.0f, 0, 10);
            this.vision = vision;
            this.parent = parent;
            
            parent.AddChild(debugLine);
        }

        Line2D debugLine = new Line2D();
        public override Vector2 getDesiredDirectionImpl()
        {
            Vector2 steer = Vector2.Zero;
            int count = 0;
            avoidDistance = vision.VisionRadius;

            // Use Physics2DDirectSpaceState for a one-off raycast
            var spaceState = parent.GetWorld2D().DirectSpaceState;
            Vector2 from = parent.GlobalPosition;
            Vector2 to = from + Vector2.Right.Rotated(parent.Rotation) * avoidDistance;

            // Create a Line2D to visualize the raycast
            debugLine.Width = 2;
            debugLine.DefaultColor = new Color(0, 1, 0); // Green
            debugLine.Points = new Vector2[] { parent.ToLocal(from), parent.ToLocal(to) };

            var query = new Godot.PhysicsRayQueryParameters2D
            {
                From = from,
                To = to,
                CollisionMask = 2 // Only detect bodies on layer 2
            };
            var result = spaceState.IntersectRay(query);



            if (result.Count > 0 && result.ContainsKey("position"))
            {
                Vector2 collisionPoint = (Vector2)result["position"];
                lastClosestWorldPoint = collisionPoint;
                Vector2 toWall = collisionPoint - parent.GlobalPosition;
                float dist = toWall.Length();

                if (dist < avoidDistance)
                {
                    steer -= toWall.Normalized() * (avoidDistance - dist) / avoidDistance;
                    steer -= toWall.Normalized();
                    count++;
                    GD.Print($"[WallAvoidance] Steering adjustment applied. Steer: {steer}");
                }
            }


            if (count > 0)
            {
                steer /= count;
                return steer.Normalized();
            }
            return Vector2.Inf;
        }

        // Helper for closest point on convex polygon
        private Vector2 GetClosestPointOnConvexPolygon(Vector2[] points, Vector2 p)
        {
            float minDist = float.MaxValue;
            Vector2 closest = Vector2.Zero;
            int n = points.Length;
            for (int i = 0; i < n; i++)
            {
                Vector2 a = points[i];
                Vector2 b = points[(i + 1) % n];
                Vector2 pt = ClosestPointOnSegment(a, b, p);
                float dist = (pt - p).LengthSquared();
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = pt;
                }
            }
            return closest;
        }

        // Helper for closest point on a segment
        private Vector2 ClosestPointOnSegment(Vector2 a, Vector2 b, Vector2 p)
        {
            Vector2 ab = b - a;
            float t = ab.Dot(p - a) / ab.LengthSquared();
            t = Mathf.Clamp(t, 0, 1);
            return a + ab * t;
        }

        // Call this from your agent's _Draw() method
        public void DrawClosestPoint()
        {
            if (lastClosestWorldPoint.HasValue)
            {
                // Draw a red circle at the closest point
                parent.DrawCircle(parent.ToLocal(lastClosestWorldPoint.Value), 8, new Color(1, 0, 0));
            }
        }
    }
}
