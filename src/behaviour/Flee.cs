using Godot;

namespace Behaviours
{
    public class Flee : Behaviour
    {
        Node2D target;
        Node2D parent;
        public Flee(string name, Node2D _target, Node2D _parent)
        {
            this.weight = PersistentParameter.ParameterRegistry.GetFloatParameter($"{name}.FleeWeight", 1.0f, 0, 10);
            this.target = _target;
            this.parent = _parent;
        }
        public override Vector2 getDesiredDirectionImpl()
        {
            return -(target.Position - parent.Position);
        }
    }
}
