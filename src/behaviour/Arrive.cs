using Godot;

namespace Behaviours
{
    public class Arrive : Behaviour
    {
        Node2D target;
        Node2D parent;
        public float slowingRadius = 100f;

        public Arrive(string name, Node2D _target, Node2D _parent)
        {
            this.weight = PersistentParameter.ParameterRegistry.GetFloatParameter($"{name}.ArriveWeight", 1.0f, 0, 10);
            this.target = _target;
            this.parent = _parent;
        }

    public override Vector2 getDesiredDirectionImpl()
        {
            Vector2 toTarget = target.Position - parent.Position;
            float distance = toTarget.Length();
            if (distance < 0.01f) return Vector2.Zero;

            float ramped = 1f * (distance / slowingRadius);
            float clipped = Mathf.Min(ramped, 1f);
            Vector2 desired = toTarget.Normalized() * clipped;
            return desired;
        }
    }
}
