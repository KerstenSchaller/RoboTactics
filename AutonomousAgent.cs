using Behaviours;
using Godot;
using PersistentParameter;
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
    {
        maxSpeed = PersistentParameter.ParameterRegistry.GetFloatParameter($"{name}.MaxSpeed", _maxSpeed,0,600);
        maxForce = PersistentParameter.ParameterRegistry.GetFloatParameter($"{name}.MaxForce", _maxForce/(2*60),0,50);
        mass = PersistentParameter.ParameterRegistry.GetFloatParameter($"{name}.Mass", _mass,0,100);
        // Randomize velocity, keeping total length at 50
        var rand = new Random();
        double angle = rand.NextDouble() * Math.PI * 2;
        velocity = new Vector2((float)(Math.Cos(angle) * 50), (float)(Math.Sin(angle) * 50));


    }

    public AutonomousAgent(string name)
    {
        maxSpeed = PersistentParameter.ParameterRegistry.GetFloatParameter($"{name}.MaxSpeed", 600,0,600);
        maxForce = PersistentParameter.ParameterRegistry.GetFloatParameter($"{name}.MaxForce", 600/(2*60),0,50);
        mass = PersistentParameter.ParameterRegistry.GetFloatParameter($"{name}.Mass", 1,0,100);
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