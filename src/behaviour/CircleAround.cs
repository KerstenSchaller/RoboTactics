using Godot;

namespace Behaviours
{
    public class CircleAround : Behaviour
    {
        Node2D target;
        Node2D parent;
        float desiredDistance;
        float distanceTolerance;
        bool increasedTolerance = false;
        private float circleShiftDeg = 30;


        public CircleAround(Node2D _target, Node2D _parent, BehaviorSet behaviorSet, float _desiredDistance = 500.0f, float _distanceTolerance = 5.0f) : base(behaviorSet)
        {
            this.weight = PersistentParameter.ParameterRegistry.GetFloatParameter($"{behaviorSet.Name}.CircleAroundWeight", 1.0f, 0, 10);
            this.target = _target;
            this.parent = _parent;
            this.desiredDistance = PersistentParameter.ParameterRegistry.GetFloatParameter($"{behaviorSet.Name}.CircleAroundDesiredDistance", _desiredDistance, 0, 1000);
            this.distanceTolerance = desiredDistance * 0.01f; // factor influences actual acquired distance
        }

        public override Vector2 getDesiredDirectionImpl()
        {
            Vector2 toTarget = target.GlobalPosition - parent.GlobalPosition;
            float currentDistance = toTarget.Length();
            float diff = currentDistance - desiredDistance;

            // Use increased tolerance if set, otherwise use the base tolerance
            float currentTol = increasedTolerance ? distanceTolerance * 1.5f : distanceTolerance;

            if (Mathf.Abs(diff) <= currentTol)
            {
                // At desired distance, move orthogonally
                GD.Print("At desired distance");
                // Slightly increase tolerance to avoid jumping
                if (!increasedTolerance)
                    increasedTolerance = true;
                return toTarget.Orthogonal();
            }
            else
            {
                // Reset increased tolerance when outside the increased range
                increasedTolerance = false;
                // Move toward a point on the toTarget vector at desiredDistance from the target,
                // but shift it along the circle by rotating the direction vector by a small angle
                Vector2 direction = toTarget.Normalized();
                float angle = circleShiftDeg * Mathf.Pi / 180.0f; // 15 degree shift in radians
                // Choose direction of rotation (clockwise or counterclockwise)
                Vector2 rotatedDirection = direction.Rotated(angle);
                Vector2 desiredPoint = target.Position - rotatedDirection * desiredDistance;
                Vector2 toDesiredPoint = desiredPoint - parent.Position;
                GD.Print("Moving towards shifted desired point on circle");
                return toDesiredPoint;
            }
        }




    }
}
