using Godot;

namespace Behaviours
{
    public class Separation : Behaviour
    {
        private Vision vision;
        private Node2D parent;
        public float separationDistance = 60f;

        public Separation(Vision vision, Node2D parent)
        {
            this.vision = vision;
            this.parent = parent;
        }

        public override Vector2 getDesiredDirectionImpl()
        {
            var neighbors = vision.GetCharacterBodiesInSight();
            if (neighbors.Count == 0)
                return Vector2.Zero;

            Vector2 steer = Vector2.Zero;
            int count = 0;
            foreach (var body in neighbors)
            {
                if (body != parent)
                {
                    float dist = parent.GlobalPosition.DistanceTo(body.GlobalPosition);
                    if (dist < separationDistance && dist > 0)
                    {
                        steer += (parent.GlobalPosition - body.GlobalPosition).Normalized() / dist;
                        count++;
                    }
                    else
                    {
                        return Vector2.Inf; // If any neighbor is too far, we return inf as a signal to avoid steering
                    }
                }
            }
            if (count == 0)
                return Vector2.Zero;

            steer /= count;
            return steer;
        }
    }
}
