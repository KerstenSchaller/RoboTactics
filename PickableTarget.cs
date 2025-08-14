using Godot;
using System;

public partial class PickableTarget : Node2D
{
    private bool _dragging = false;
    private Vector2 _dragOffset;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (_dragging)
        {
            GlobalPosition = GetGlobalMousePosition() + _dragOffset;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
            {
                // Check if mouse is over this node (using its global position and a small radius)
                Vector2 mousePos = GetGlobalMousePosition();
                if (GlobalPosition.DistanceTo(mousePos) < 32) // 32 pixels radius, adjust as needed
                {
                    _dragging = true;
                    _dragOffset = GlobalPosition - mousePos;
                    Input.MouseMode = Input.MouseModeEnum.Visible;
                }
            }
            else if (!mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
            {
                _dragging = false;
            }
        }
    }
}
