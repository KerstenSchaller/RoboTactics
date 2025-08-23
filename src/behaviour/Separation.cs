using Godot;

namespace Behaviours
{
    public class Separation : Behaviour
    {
        private Vision vision;
        private Node2D parent;

        public Separation(string name, Vision vision, Node2D parent)
        {
            this.weight = PersistentParameter.ParameterRegistry.GetFloatParameter($"{name}.SeparationWeight", 1.0f, 0.0f, 10.0f);
            this.vision = vision;
            this.parent = parent;
        }

        public override Vector2 getDesiredDirectionImpl()
        {
            var neighbors = vision.GetCharacterBodiesInSight();
            if (neighbors.Count == 0)
                return Vector2.Inf;

            Vector2 steer = Vector2.Zero;
            int count = 0;
            int maxNeighbors = 5;
            foreach (var body in neighbors)
            {
                if (body != parent)
                {
                    float dist = parent.GlobalPosition.DistanceTo(body.GlobalPosition);
                    if (dist > 0)
                    {
                        steer += (parent.GlobalPosition - body.GlobalPosition).Normalized() / dist;
                        count++;
                    }
                }
                if (count >= maxNeighbors)break;

            }

            steer /= count;
            return steer;
        }
    }
}
