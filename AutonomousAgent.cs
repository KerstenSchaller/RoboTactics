using Behaviours;
using Godot;
using System;
using System.Collections.Generic;



class AutonomousAgent
{
    public float Mass { get => mass; set => mass = value; }
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
    public float MaxForce { get => maxForce; set => maxForce = value; }
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
                    applyForce(desired);
                }
            }
            return velocity;
        }
    }

    float mass;
    float maxSpeed;
    float maxForce;
    Vector2 acceleration;
    Vector2 velocity;

    List<Behaviours.Behaviour> behaviours = new List<Behaviours.Behaviour>();

    public AutonomousAgent(float _maxSpeed, float _maxForce, float _mass)
    {
        maxSpeed = _maxSpeed;
        maxForce = _maxForce;
        mass = _mass;
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

    public void applyForce(Vector2 desired)
    {
        var steering = desired.Normalized()*maxSpeed - velocity;
        steering = (steering.Length() >= maxForce) ? steering.Normalized()*maxForce:steering;

        acceleration += steering/mass;
        velocity += acceleration;
        //GD.Print("s: " + velocity.Length() + " f: " + steering.Length() + " mxF: " + maxForce);

    }
}