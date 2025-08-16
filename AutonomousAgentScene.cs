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
        Wander,
        Arrive,
        Pursue,
        Evade,
        Cohesion,
        Alignment,
        Separation
    }
    AutonomousAgent autonomousAgent;
    [Export] Node2D targetNode;
    [Export] Area2D visionArea;
    [Export] CharacterBody2D characterBody;

    // Exported bools for each behaviour

    [Export]
    public bool EnableSeek { get; set; } = false;
    [Export]
    public bool EnableFlee { get; set; } = false;
    [Export]
    public bool EnableCircleAround { get; set; } = false;
    [Export]
    public bool EnableWander { get; set; } = false;
    [Export]
    public bool EnableArrive { get; set; } = false;
    [Export]
    public bool EnablePursue { get; set; } = false;
    [Export]
    public bool EnableEvade { get; set; } = false;
    [Export]
    public bool EnableCohesion { get; set; } = false;
    [Export]
    public bool EnableAlignment { get; set; } = false;
    [Export]
    public bool EnableSeparation { get; set; } = false;

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

    public override void _Ready()
    {
        autonomousAgent = new AutonomousAgent(300.0f, 1000.0f, 0.5f);

        // Instantiate all behaviours
        seekBehaviour = new Behaviours.Seek(targetNode, characterBody);
        fleeBehaviour = new Behaviours.Flee(targetNode, characterBody);
        circleAroundBehaviour = new Behaviours.CircleAround(targetNode, characterBody);
        wanderBehaviour = new Behaviours.Wander(targetNode, characterBody);
        arriveBehaviour = new Behaviours.Arrive(targetNode, characterBody);
        pursueBehaviour = new Behaviours.Pursue(targetNode, characterBody);
        evadeBehaviour = new Behaviours.Evade(targetNode, characterBody);
        cohesionBehaviour = new Behaviours.Cohesion(visionArea as Vision, characterBody);
        alignmentBehaviour = new Behaviours.Alignment(visionArea as Vision, characterBody);
        separationBehaviour = new Behaviours.Separation(visionArea as Vision, characterBody);

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
        if(characterBody == null || autonomousAgent == null)
        {
            GD.PrintErr($"[{nameof(AutonomousAgentScene)}] ({nameof(_Process)}): CharacterBody or AutonomousAgent is not set. File: {nameof(AutonomousAgentScene)}.cs");
            return;
        }
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
        }
    }
// ...existing code...
}
