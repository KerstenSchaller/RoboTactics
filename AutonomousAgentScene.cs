using Behaviours;
using Godot;
using System;

public partial class AutonomousAgentScene : CharacterBody2D
{
    // Enum for behaviour types
    public enum BehaviourType
    {
        Seek,
        Flee,
        CircleAround,
        Wander,
        Arrive,
        Pursue,
        Evade,
        Cohesion,
        Alignment,
        Separation,
        WallAvoidance,
        GroupLimiter
    }
    AutonomousAgent autonomousAgent;
    [Export] Node2D targetNode;
    [Export] Area2D visionArea;

    // Exported bools for each behaviour
    private bool _enableGroupLimiter;
    [Export]
    public bool EnableGroupLimiter
    {
        get => _enableGroupLimiter;
        set
        {
            if (_enableGroupLimiter != value)
            {
                _enableGroupLimiter = value;
                SetBehaviourEnabled(BehaviourType.GroupLimiter, value);
            }
        }
    }

    private bool _enableWallAvoidance;
    [Export]
    public bool EnableWallAvoidance
    {
        get => _enableWallAvoidance;
        set
        {
            if (_enableWallAvoidance != value)
            {
                _enableWallAvoidance = value;
                SetBehaviourEnabled(BehaviourType.WallAvoidance, value);
            }
        }
    }

    private bool _enableSeek;
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

    private bool _enableFlee;
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

    private bool _enableCircleAround;
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

    private bool _enableWander;
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

    private bool _enableArrive;
    [Export]
    public bool EnableArrive
    {
        get => _enableArrive;
        set
        {
            if (_enableArrive != value)
            {
                _enableArrive = value;
                SetBehaviourEnabled(BehaviourType.Arrive, value);
            }
        }
    }

    private bool _enablePursue;
    [Export]
    public bool EnablePursue
    {
        get => _enablePursue;
        set
        {
            if (_enablePursue != value)
            {
                _enablePursue = value;
                SetBehaviourEnabled(BehaviourType.Pursue, value);
            }
        }
    }

    private bool _enableEvade;
    [Export]
    public bool EnableEvade
    {
        get => _enableEvade;
        set
        {
            if (_enableEvade != value)
            {
                _enableEvade = value;
                SetBehaviourEnabled(BehaviourType.Evade, value);
            }
        }
    }

    private bool _enableCohesion;
    [Export]
    public bool EnableCohesion
    {
        get => _enableCohesion;
        set
        {
            if (_enableCohesion != value)
            {
                _enableCohesion = value;
                SetBehaviourEnabled(BehaviourType.Cohesion, value);
            }
        }
    }

    private bool _enableAlignment;
    [Export]
    public bool EnableAlignment
    {
        get => _enableAlignment;
        set
        {
            if (_enableAlignment != value)
            {
                _enableAlignment = value;
                SetBehaviourEnabled(BehaviourType.Alignment, value);
            }
        }
    }

    private bool _enableSeparation;
    [Export]
    public bool EnableSeparation
    {
        get => _enableSeparation;
        set
        {
            if (_enableSeparation != value)
            {
                _enableSeparation = value;
                SetBehaviourEnabled(BehaviourType.Separation, value);
            }
        }
    }

    // Behaviour instances
    private Behaviours.Seek seekBehaviour;
    private Behaviours.Flee fleeBehaviour;
    private Behaviours.CircleAround circleAroundBehaviour;
    private Behaviours.Wander wanderBehaviour;
    private Behaviours.Arrive arriveBehaviour;
    private Behaviours.Pursue pursueBehaviour;
    private Behaviours.Evade evadeBehaviour;
    private Behaviours.Cohesion cohesionBehaviour;
    private Behaviours.Alignment alignmentBehaviour;
    private Behaviours.Separation separationBehaviour;
    private Behaviours.WallAvoidance wallAvoidanceBehaviour;
    private Behaviours.GroupLimiter groupLimiterBehaviour;

    public override void _Ready()
    {
        float speed = 450.0f;
        int secondsTillFullSpeed = 2;
        autonomousAgent = new AutonomousAgent("boidAgent", speed, speed / (secondsTillFullSpeed * 60), 0.5f);

        // Instantiate all behaviours
        seekBehaviour = new Behaviours.Seek("boidAgent", targetNode, this);
        fleeBehaviour = new Behaviours.Flee("boidAgent", targetNode, this);
        circleAroundBehaviour = new Behaviours.CircleAround("boidAgent", targetNode, this);
        wanderBehaviour = new Behaviours.Wander("boidAgent", targetNode, this);
        arriveBehaviour = new Behaviours.Arrive("boidAgent", targetNode, this);
        pursueBehaviour = new Behaviours.Pursue("boidAgent", targetNode, this);
        evadeBehaviour = new Behaviours.Evade("boidAgent", targetNode, this);
        cohesionBehaviour = new Behaviours.Cohesion("boidAgent", visionArea as Vision, this);
        alignmentBehaviour = new Behaviours.Alignment("boidAgent", visionArea as Vision, this);
        separationBehaviour = new Behaviours.Separation("boidAgent", visionArea as Vision, this);
        wallAvoidanceBehaviour = new Behaviours.WallAvoidance("boidAgent", visionArea as Vision, this);
        groupLimiterBehaviour = new Behaviours.GroupLimiter("boidAgent", visionArea as Vision, this);

        // Add all behaviours to the agent
        autonomousAgent.addBehaviour(seekBehaviour);
        autonomousAgent.addBehaviour(fleeBehaviour);
        autonomousAgent.addBehaviour(circleAroundBehaviour);
        autonomousAgent.addBehaviour(wanderBehaviour);
        autonomousAgent.addBehaviour(arriveBehaviour);
        autonomousAgent.addBehaviour(pursueBehaviour);
        autonomousAgent.addBehaviour(evadeBehaviour);
        autonomousAgent.addBehaviour(cohesionBehaviour);
        autonomousAgent.addBehaviour(alignmentBehaviour);
        autonomousAgent.addBehaviour(separationBehaviour);
        autonomousAgent.addBehaviour(wallAvoidanceBehaviour);
        autonomousAgent.addBehaviour(groupLimiterBehaviour);


        // Set enabled state from exported bools
        SetBehaviourEnabled(BehaviourType.Seek, EnableSeek);
        SetBehaviourEnabled(BehaviourType.Flee, EnableFlee);
        SetBehaviourEnabled(BehaviourType.CircleAround, EnableCircleAround);
        SetBehaviourEnabled(BehaviourType.Wander, EnableWander);
        SetBehaviourEnabled(BehaviourType.Arrive, EnableArrive);
        SetBehaviourEnabled(BehaviourType.Pursue, EnablePursue);
        SetBehaviourEnabled(BehaviourType.Evade, EnableEvade);
        SetBehaviourEnabled(BehaviourType.Cohesion, EnableCohesion);
        SetBehaviourEnabled(BehaviourType.Alignment, EnableAlignment);
        SetBehaviourEnabled(BehaviourType.Separation, EnableSeparation);
        SetBehaviourEnabled(BehaviourType.WallAvoidance, EnableWallAvoidance);
        SetBehaviourEnabled(BehaviourType.GroupLimiter, EnableGroupLimiter);
        
        this.AddChild(autonomousAgent);
    }

    // Dynamically add a behaviour to the agent
    public void AddBehaviour(Behaviour behaviour)
    {
        autonomousAgent.addBehaviour(behaviour);
    }



    public override void _PhysicsProcess(double delta)
    {
        if (autonomousAgent == null)
        {
            return;
        }
        Velocity = autonomousAgent.Velocity;
        this.Rotation = Velocity.Angle();
        //this.Position += Velocity * (float)delta;
        MoveAndSlide();
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
            case BehaviourType.Arrive:
                if (arriveBehaviour != null) arriveBehaviour.Enabled = enabled;
                break;
            case BehaviourType.Pursue:
                if (pursueBehaviour != null) pursueBehaviour.Enabled = enabled;
                break;
            case BehaviourType.Evade:
                if (evadeBehaviour != null) evadeBehaviour.Enabled = enabled;
                break;
            case BehaviourType.Cohesion:
                if (cohesionBehaviour != null) cohesionBehaviour.Enabled = enabled;
                break;
            case BehaviourType.Alignment:
                if (alignmentBehaviour != null) alignmentBehaviour.Enabled = enabled;
                break;
            case BehaviourType.Separation:
                if (separationBehaviour != null) separationBehaviour.Enabled = enabled;
                break;
            case BehaviourType.WallAvoidance:
                if (wallAvoidanceBehaviour != null) wallAvoidanceBehaviour.Enabled = enabled;
                break;
            case BehaviourType.GroupLimiter:
                if (groupLimiterBehaviour != null) groupLimiterBehaviour.Enabled = enabled;
                break;
        }
    }
    
        // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    public override void _Draw()
    {
        // draw velocity vector
        //DrawLine(Vector2.Zero, ToLocal(Position + Velocity) - ToLocal(Position), Colors.Red, 2);
    }
// ...existing code...
}
