using Godot;

namespace Behaviours
{
    public abstract partial class Behaviour 
    {
        protected bool enabled = false;
        protected FloatParameter weight;
        public abstract Vector2 getDesiredDirectionImpl();

        public Behaviour(BehaviorSet behaviorSet)
        {
            behaviorSet.AddBehavior(this);
        }

        public float Weight
        {
            get { return weight; }
            set { weight.Value = value; }
        }

        public Vector2 getDesiredDirection()
        {
            if (enabled)
            {
                var desired = getDesiredDirectionImpl();
                return desired;
            }
            return Vector2.Inf;
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
