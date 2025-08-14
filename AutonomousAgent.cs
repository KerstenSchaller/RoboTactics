using Behaviours;
using Godot;
using System;
using System.Collections.Generic;



namespace Behaviours
{
    public abstract class Behaviour
    {
        protected bool enabled = true;
        public abstract Vector2 getDesiredDirection();
        public virtual bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public void disable(){enabled = false;}
        public void enable(){enabled = true; }
    }

    class Seek : Behaviour
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
        public override Vector2 getDesiredDirection()
        {
            return target.Position - parent.Position;
        }
    }

    class CircleAround : Behaviour
    {
        Node2D target;
        Node2D parent;
        public CircleAround(Node2D _target, Node2D _parent)
        {
            this.target = _target;
            this.parent = _parent;
        }

        public override Vector2 getDesiredDirection()
        {
            return (target.Position - parent.Position).Orthogonal();
        }

    }

    class Flee : Behaviour
    {
        Node2D target;
        Node2D parent;
        public Flee(Node2D _target, Node2D _parent)
        {
            this.target = _target;
            this.parent = _parent;
        }
        public override Vector2 getDesiredDirection()
        {
            return -(target.Position - parent.Position);
        }
    }
}

class AutonomousAgent
{
    float mass;
    float maxSpeed;
    float maxForce;
    Vector2 acceleration;
    Vector2 velocity;

    List<Behaviours.Behaviour> behaviours = new List<Behaviours.Behaviour>();
    public Vector2 Velocity
    {
        get
        {
            acceleration = new Vector2();
            foreach(var behaviour in behaviours)
            {
                if(behaviour.Enabled)
                {
                    applyForce(behaviour.getDesiredDirection());
                }
            }
            return velocity;
        }
    }

    public AutonomousAgent(float _maxSpeed, float _maxForce, float _mass)
    {
        maxSpeed = _maxSpeed;
        maxForce = _maxForce;
        mass = _mass;
    }

    public void addBehaviour(Behaviours.Behaviour _behaviour)
    {
        if(!behaviours.Contains(_behaviour))
        {
            behaviours.Add(_behaviour);
        }
    }

    public void removeBehaviour(Behaviours.Behaviour _behaviour)
    {
        behaviours.Remove(_behaviour);
    }

    public void applyForce(Vector2 desired)
    {
        var steering = desired.Normalized()*maxSpeed - velocity;
        steering = (steering.Length() >= maxForce) ? steering.Normalized()*maxForce:steering;

        acceleration += steering/mass;
        velocity += acceleration;
        //GD.Print("s: " + velocity.Length() + " f: " + steering.Length() + " mxF: " + maxForce);

    }
}