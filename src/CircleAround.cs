using Godot;

namespace Behaviours
{
    public class CircleAround : Behaviour
    {
        Node2D target;
        Node2D parent;
        public CircleAround(Node2D _target, Node2D _parent)
        {
            this.target = _target;
            this.parent = _parent;
        }

        public override Vector2 getDesiredDirectionImpl()
        {
            return (target.Position - parent.Position).Orthogonal();
        }
    }
}
