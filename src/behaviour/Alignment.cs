using Godot;

namespace Behaviours
{
    public class Alignment : Behaviour
    {
        private Vision vision;
        private Node2D parent;

        public Alignment(string name, Vision vision, Node2D parent)
        {
            this.weight = new FloatParameter($"{name}.AlignmentWeight", 1.0f);
            this.vision = vision;
            this.parent = parent;
        }

        public override Vector2 getDesiredDirectionImpl()
        {
            var neighbors = vision.GetCharacterBodiesInSight();
            if (neighbors.Count == 0)
                return Vector2.Inf;

            Vector2 avgVelocity = Vector2.Zero;
            int count = 0;
            foreach (var body in neighbors)
            {
                if (body != parent && body is CharacterBody2D cb)
                {
                    avgVelocity += cb.Velocity;
                    count++;
                }
            }


            avgVelocity /= count;
            return avgVelocity;
        }
    }
}
