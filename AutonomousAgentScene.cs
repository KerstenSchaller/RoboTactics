using Behaviours;
using Godot;
using System;

public partial class AutonomousAgentScene : CharacterBody2D
{
    AutonomousAgent autonomousAgent;
    [Export] Node2D targetNode;
    public override void _Ready()
    {
        autonomousAgent = new AutonomousAgent(100.0f, 1000.0f, 1f);
        autonomousAgent.addBehaviour(new Seek(targetNode, this));
        //autonomousAgent.addBehaviour(new CircleAround(this, this));
        //autonomousAgent.addBehaviour(new Flee(this, this));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        this.Velocity = autonomousAgent.Velocity;
        this.MoveAndSlide();
    }
}
