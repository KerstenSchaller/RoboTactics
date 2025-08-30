using Godot;

namespace Behaviours
{
    public class Cohesion : Behaviour
    {
        private Vision vision;
        private Node2D parent;

        public Cohesion( Vision vision, Node2D parent, BehaviorSet behaviorSet) : base(behaviorSet)
        {
            this.weight = PersistentParameter.ParameterRegistry.GetFloatParameter($"{behaviorSet.Name}.CohesionWeight", 1.0f, 0, 10);
            this.vision = vision;
            this.parent = parent;
        }

        public override Vector2 getDesiredDirectionImpl()
        {
            var neighbors = vision.GetCharacterBodiesInSight();
            if (neighbors.Count == 0)
                return Vector2.Inf;

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


            center /= count;
            return (center - parent.GlobalPosition);
        }
    }
}
