using Godot;

namespace Behaviours
{
    public class Evade : Behaviour
    {
        Node2D target;
        Node2D parent;
        public float predictionFactor = 1.0f;

        public Evade(string name, Node2D _target, Node2D _parent)
        {
            this.weight = PersistentParameter.ParameterRegistry.GetFloatParameter($"{name}.EvadeWeight", 1.0f, 0, 10);
            this.target = _target;
            this.parent = _parent;
        }

    public override Vector2 getDesiredDirectionImpl()
        {
            Vector2 toTarget = target.Position - parent.Position;
            Vector2 targetVelocity = Vector2.Zero;
            if (target is CharacterBody2D cb)
                targetVelocity = cb.Velocity;

            float prediction = toTarget.Length() / (1f + 0.01f);
            Vector2 futurePosition = target.Position + targetVelocity * prediction * predictionFactor;
            return (parent.Position - futurePosition);
        }
    }
}
