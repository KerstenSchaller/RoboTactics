using Godot;

namespace Behaviours
{
    public abstract class Behaviour
    {
        protected bool enabled = false;
        public abstract Vector2 getDesiredDirectionImpl();
        public Vector2 getDesiredDirection()
        {
            if (enabled)
            {
                return getDesiredDirectionImpl();
            }
            return Vector2.Zero;
        }
        public virtual bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public void disable() { enabled = false; }
        public void enable() { enabled = true; }
    }
}
