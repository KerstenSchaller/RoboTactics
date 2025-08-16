using Godot;
using System;

namespace Behaviours
{
    public class Wander : Behaviour
    {
        // wanderTheta: initial angle for the wander direction; affects starting direction
        private double wanderTheta = Mathf.DegToRad(90);

        // wanderDistance: how far ahead the wander circle is placed; higher = more forward
        private double wanderDistance = 50;

        // wanderRadius: size of the wander circle; higher = more erratic movement
        private double wanderRadius = 90;

        // displacementRangeDegree: max angle change per update; higher = more random turns
        private double displacementRangeDegree = 20;

        private Node2D parent;

        public Wander(Node2D _target, Node2D _parent)
        {
            this.parent = _parent;
        }

        public override Vector2 getDesiredDirectionImpl()
        {
            Vector2 rot = new Vector2((float)Math.Cos(parent.Rotation), (float)Math.Sin(parent.Rotation));
            Vector2 circleCenter = rot.Normalized() * (float)wanderDistance;
            Vector2 displacement = new Vector2((float)Math.Cos(wanderTheta), (float)Math.Sin(wanderTheta)) * (float)wanderRadius;
            double randomDisplacement = (GD.Randf() * 2 - 1) * Mathf.DegToRad((float)displacementRangeDegree);
            wanderTheta += randomDisplacement;
            Vector2 wanderForce = circleCenter + displacement;
            return wanderForce;
        }
    }
}
