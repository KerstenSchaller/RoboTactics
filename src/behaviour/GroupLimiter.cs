using Godot;
using PersistentParameter;

namespace Behaviours
{
    public class GroupLimiter : Behaviour
    {
        private Vision vision;
        private Node2D parent;
        private IntParameter maxGroupSize;
        private FloatParameter minDistance;

        // State for persistent fleeing
        private bool isFleeing = false;
        private Vector2 lastFleeCenter = Vector2.Zero;

        public GroupLimiter( Vision vision, Node2D parent, BehaviorSet behaviorSet) : base(behaviorSet)
        {
            this.maxGroupSize = ParameterRegistry.GetIntParameter($"{behaviorSet.Name}.GroupLimiterMaxGroupSize", 10, 1, 100);
            this.minDistance = ParameterRegistry.GetFloatParameter($"{behaviorSet.Name}.GroupLimiterMinDistance", 500f, 100f, 1000f);
            this.vision = vision;
            this.parent = parent;
            this.weight = ParameterRegistry.GetFloatParameter($"{behaviorSet.Name}.GroupLimiterWeight", 1.0f, 0, 10);
        }

        public override Vector2 getDesiredDirectionImpl()
        {
            var neighbors = vision.GetCharacterBodiesInSight();

            // If already fleeing, keep fleeing until distance is reached
            if (isFleeing)
            {
                float fleeDist = parent.GlobalPosition.DistanceTo(lastFleeCenter);
                if (fleeDist < minDistance.Value)
                {
                    return (parent.GlobalPosition - lastFleeCenter).Normalized() * (minDistance.Value - fleeDist);
                }
                else
                {
                    isFleeing = false;
                    return Vector2.Inf;
                }
            }

            // If not fleeing, check if group size exceeded
            if (neighbors.Count > maxGroupSize.Value)
            {
                // Calculate center only once at the start of fleeing
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
                if (count > 0)
                {
                    center /= count;
                    isFleeing = true;
                    lastFleeCenter = center;
                    float fleeDist = parent.GlobalPosition.DistanceTo(lastFleeCenter);
                    if (fleeDist < minDistance.Value)
                    {
                        return (parent.GlobalPosition - lastFleeCenter).Normalized() * (minDistance.Value - fleeDist);
                    }
                }
            }

            return Vector2.Inf;
        }
    }
}
