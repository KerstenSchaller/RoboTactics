using Godot;

namespace Behaviours
{
    public class WallAvoidance : Behaviour
    {
        private Vision vision;
        private Node2D parent;
        private float avoidDistance; // Distance to start avoiding

        private Vector2? lastClosestWorldPoint = null;

        public WallAvoidance( Vision vision, Node2D parent, BehaviorSet behaviorSet) : base(behaviorSet)
        {
            this.weight = PersistentParameter.ParameterRegistry.GetFloatParameter($"{behaviorSet.Name}.WallAvoidanceWeight", 1.0f, 0, 10);
            this.vision = vision;
            this.parent = parent;
            
        }

        public override Vector2 getDesiredDirectionImpl()
        {
            Vector2 steer = Vector2.Zero;
            int count = 0;
            avoidDistance = vision.VisionRadius.Value;

            Vector2 from = parent.GlobalPosition;
            Vector2 to = from + Vector2.Right.Rotated(parent.Rotation) * avoidDistance;

        
            // Use Vision's Raycast method
            var result = vision.Raycast(from, to, 2);


            foreach (var hit in result)
            {
                if (hit.Count > 0 && hit.ContainsKey("position"))
                {
                    Vector2 collisionPoint = (Vector2)hit["position"];
                    lastClosestWorldPoint = collisionPoint;
                    Vector2 toWall = collisionPoint - parent.GlobalPosition;
                    float dist = toWall.Length();

                    if (dist < avoidDistance)
                    {
                        steer -= toWall.Normalized() * (avoidDistance - dist) / avoidDistance;
                        steer -= toWall.Normalized();
                        count++;

                    }
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

    }
}
