using Godot;
using System;

namespace Behaviours
{
    public class Wander : Behaviour
    {
        private double wanderTheta = Mathf.DegToRad(90);
        private double wanderDistance = 150;
        private double wanderRadius = 90;
        private double displacementRangeDegree = 20;

        private Node2D parent;

        public Wander(Node2D _target, Node2D _parent)
        {
            this.parent = _parent;
        }

        public override Vector2 getDesiredDirectionImpl()
        {
            Vector2 circleCenter = parent.Transform.X.Normalized() * (float)wanderDistance;
            Vector2 displacement = new Vector2((float)Math.Cos(wanderTheta), (float)Math.Sin(wanderTheta)) * (float)wanderRadius;
            double randomDisplacement = (GD.Randf() * 2 - 1) * Mathf.DegToRad((float)displacementRangeDegree);
            wanderTheta += randomDisplacement;
            Vector2 wanderForce = circleCenter + displacement;
            return wanderForce;
        }
    }
}
