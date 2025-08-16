using Godot;
using System;

[Tool]
public partial class Main : Node2D
{
    // Called when the node enters the scene tree for the first time.
    private PackedScene _agentScene;

    public override void _Ready()
    {
        // Load the AutonomousAgentScene.tscn once
        _agentScene = GD.Load<PackedScene>("res://AutonomousAgentScene.tscn");
        if (_agentScene == null)
        {
            GD.PrintErr("Failed to load AutonomousAgentScene.tscn");
            return;
        }

        for (int i = 0; i < 80; i++)
        {
            InstantiateAgent(i);
        }
    }

    private void InstantiateAgent(int index)
    {
        var agentInstance = _agentScene.Instantiate();
        if (agentInstance is Node2D node2D)
        {
            // Find the Camera2D child node
            var camera = GetNodeOrNull<Camera2D>("Camera2D");
            if (camera != null)
            {
                // Get the camera's visible rectangle in world coordinates
                Rect2 cameraRect = camera.GetViewportRect();
                Vector2 topLeft = camera.GetScreenCenterPosition() - cameraRect.Size / 2;
                var random = new Random();
                float x = (float)random.NextDouble() * cameraRect.Size.X + topLeft.X;
                float y = (float)random.NextDouble() * cameraRect.Size.Y + topLeft.Y;
                node2D.Position = new Vector2(x, y);
            }

        }
        AddChild(agentInstance);
    }
    

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
