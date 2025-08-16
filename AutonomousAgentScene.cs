using Behaviours;
using Godot;
using System;

public partial class AutonomousAgentScene : Node2D
{
    // Enum for behaviour types
    public enum BehaviourType
    {
        Seek,
        Flee,
        CircleAround,
        Wander
    }
    AutonomousAgent autonomousAgent;
    [Export] Node2D targetNode;
    [Export] Area2D visionArea;
    [Export] CharacterBody2D characterBody;

    // Exported bools for each behaviour

    private bool _enableSeek = true;
    private bool _enableFlee = false;
    private bool _enableCircleAround = false;
    private bool _enableWander = false;


    [Export]
    public bool EnableSeek
    {
        get => _enableSeek;
        set
        {
            if (_enableSeek != value)
            {
                _enableSeek = value;
                SetBehaviourEnabled(BehaviourType.Seek, value);
            }
        }
    }
    [Export]
    public bool EnableFlee
    {
        get => _enableFlee;
        set
        {
            if (_enableFlee != value)
            {
                _enableFlee = value;
                SetBehaviourEnabled(BehaviourType.Flee, value);
            }
        }
    }
    [Export]
    public bool EnableCircleAround
    {
        get => _enableCircleAround;
        set
        {
            if (_enableCircleAround != value)
            {
                _enableCircleAround = value;
                SetBehaviourEnabled(BehaviourType.CircleAround, value);
            }
        }
    }
    [Export]
    public bool EnableWander
    {
        get => _enableWander;
        set
        {
            if (_enableWander != value)
            {
                _enableWander = value;
                SetBehaviourEnabled(BehaviourType.Wander, value);
            }
        }
    }

    // Behaviour instances
    private Behaviours.Seek seekBehaviour;
    private Behaviours.Flee fleeBehaviour;
    private Behaviours.CircleAround circleAroundBehaviour;
    private Behaviours.Wander wanderBehaviour;

    public override void _Ready()
    {
        autonomousAgent = new AutonomousAgent(100.0f, 1000.0f, 1f);

        // Instantiate all behaviours
    seekBehaviour = new Behaviours.Seek(targetNode, characterBody);
    fleeBehaviour = new Behaviours.Flee(targetNode, characterBody);
    circleAroundBehaviour = new Behaviours.CircleAround(targetNode, characterBody);
    wanderBehaviour = new Behaviours.Wander(targetNode, characterBody);

        // Add all behaviours to the agent
        autonomousAgent.addBehaviour(seekBehaviour);
        autonomousAgent.addBehaviour(fleeBehaviour);
        autonomousAgent.addBehaviour(circleAroundBehaviour);
        autonomousAgent.addBehaviour(wanderBehaviour);

        // Set enabled state from exported bools (triggers events)
        EnableSeek = _enableSeek;
        EnableFlee = _enableFlee;
        EnableCircleAround = _enableCircleAround;
        EnableWander = _enableWander;
    }

    // Dynamically add a behaviour to the agent
    public void AddBehaviour(Behaviour behaviour)
    {
        autonomousAgent.addBehaviour(behaviour);
    }

    // Configure agent parameters at runtime
    public void ConfigureAgent(float maxSpeed, float maxForce, float mass)
    {
        autonomousAgent.ConfigureAgent(maxSpeed, maxForce, mass);
    }

    public override void _Process(double delta)
    {
        characterBody.Velocity = autonomousAgent.Velocity;
        characterBody.MoveAndSlide();
    }

    // Set behaviour enabled state by name, can be called from outside code
    public void SetBehaviourEnabled(BehaviourType behaviourType, bool enabled)
    {
        switch (behaviourType)
        {
            case BehaviourType.Seek:
                if (seekBehaviour != null) seekBehaviour.Enabled = enabled;
                break;
            case BehaviourType.Flee:
                if (fleeBehaviour != null) fleeBehaviour.Enabled = enabled;
                break;
            case BehaviourType.CircleAround:
                if (circleAroundBehaviour != null) circleAroundBehaviour.Enabled = enabled;
                break;
            case BehaviourType.Wander:
                if (wanderBehaviour != null) wanderBehaviour.Enabled = enabled;
                break;
        }
    }
// ...existing code...
}
