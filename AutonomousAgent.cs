using Behaviours;
using Godot;
using System;
using System.Collections.Generic;



class AutonomousAgent
{
    public float Mass { get => mass; set => mass.Value = value; }
    public float MaxSpeed { get => maxSpeed; set => maxSpeed.Value = value; }
    public float MaxForce { get => maxForce; set => maxForce.Value = value; }
    public Vector2 Velocity
    {
        get
        {
            acceleration = new Vector2();
            foreach(var behaviour in behaviours)
            {
                if(behaviour.Enabled)
                {
                    Vector2 desired = behaviour.getDesiredDirection();
                    if(desired != Vector2.Inf)
                    {
                        // only if valid desired direction
                        applyForce(desired, behaviour.Weight);
                    }
                }
            }
            velocity += acceleration;
            if (velocity.Length() > maxSpeed)
            {
                velocity = velocity.Normalized() * maxSpeed;
            }
            return velocity;
        }
    }

    FloatParameter mass;
    FloatParameter maxSpeed;
    FloatParameter maxForce;
    Vector2 acceleration;
    Vector2 velocity;

    List<Behaviours.Behaviour> behaviours = new List<Behaviours.Behaviour>();

    public AutonomousAgent(string name,float _maxSpeed, float _maxForce, float _mass)
        : this(name)
    {
        // overwrite values and save parameters
        maxSpeed.Value = _maxSpeed; 
        maxForce.Value = _maxForce;
        mass.Value = _mass;

        // Randomize velocity, keeping total length at 50
        var rand = new Random();
        double angle = rand.NextDouble() * Math.PI * 2;
        velocity = new Vector2((float)(Math.Cos(angle) * 50), (float)(Math.Sin(angle) * 50));

        MaxSpeed = _maxSpeed;
        MaxForce = _maxForce;
        Mass = _mass;
    }

    public AutonomousAgent(string name)
    {
        maxSpeed = new FloatParameter($"{name}.MaxSpeed", 600);
        maxForce = new FloatParameter($"{name}.MaxForce", 600/(2*60));
        mass = new FloatParameter($"{name}.Mass", 1);
    }

    public void ConfigureAgent(float maxSpeed, float maxForce, float mass)
    {
        this.MaxSpeed = maxSpeed;
        this.MaxForce = maxForce;
        this.Mass = mass;
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

    public void applyForce(Vector2 desired, float Weight)
    {
        var steering = desired.Normalized()*maxSpeed - velocity;
        steering = (steering.Length() >= maxForce) ? steering.Normalized()*maxForce:steering;

        acceleration += (steering/mass)*Weight;
        

    }
}