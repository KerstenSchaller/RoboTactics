using Godot;

namespace Behaviours
{
    public class Seek : Behaviour
    {
        Node2D target;
        Node2D parent;
        public Seek(Node2D _target, Node2D _parent)
        {
            this.target = _target;
            this.parent = _parent;
        }

        public void changeTarget(Node2D _target)
        {
            this.target = _target;
        }
        public override Vector2 getDesiredDirectionImpl()
        {
            return target.Position - parent.Position;
        }
    }
}
