using Godot;

namespace Behaviours
{
    public class Cohesion : Behaviour
    {
        private Vision vision;
        private Node2D parent;

        public Cohesion(Vision vision, Node2D parent)
        {
            this.vision = vision;
            this.parent = parent;
        }

        public override Vector2 getDesiredDirectionImpl()
        {
            var neighbors = vision.GetCharacterBodiesInSight();
            if (neighbors.Count == 0)
                return Vector2.Zero;

            Vector2 center = Vector2.Zero;
            int count = 0;
            foreach (var body in neighbors)
            {
                if (body != parent)
                {
                    center += body.GlobalPosition;
                    count++;
                }
            }
            if (count == 0)
                return Vector2.Zero;

            center /= count;
            return (center - parent.GlobalPosition);
        }
    }
}
