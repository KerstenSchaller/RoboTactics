using Godot;

namespace Behaviours
{
    public class CircleAround : Behaviour
    {
        Node2D target;
        Node2D parent;
        public CircleAround(string name, Node2D _target, Node2D _parent)
        {
            this.weight = PersistentParameter.ParameterRegistry.GetFloatParameter($"{name}.CircleAroundWeight", 1.0f, 0, 10);
            this.target = _target;
            this.parent = _parent;
        }

        public override Vector2 getDesiredDirectionImpl()
        {
            return (target.Position - parent.Position).Orthogonal();
        }
    }
}
