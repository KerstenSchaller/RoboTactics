using Godot;
using System;

public partial class ControllableCamera2d : Camera2D
{
    // Called when the node enters the scene tree for the first time.

    private bool _dragging = false;
    private Vector2 _lastMousePosition;
    private float _moveSpeed = 600f;

    public override void _Ready()
    {
        // Optionally, set process input
        SetProcessInput(true);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

        Vector2 direction = Vector2.Zero;
        if (Input.IsActionPressed("ui_up") || Input.IsKeyPressed(Key.W))
            direction.Y -= 1;
        if (Input.IsActionPressed("ui_down") || Input.IsKeyPressed(Key.S))
            direction.Y += 1;
        if (Input.IsActionPressed("ui_left") || Input.IsKeyPressed(Key.A))
            direction.X -= 1;
        if (Input.IsActionPressed("ui_right") || Input.IsKeyPressed(Key.D))
            direction.X += 1;

        if (direction != Vector2.Zero)
        {
            Position += direction.Normalized() * _moveSpeed * (float)delta;
        }

        // Mouse drag movement
        if (_dragging)
        {
            Vector2 mousePos = GetViewport().GetMousePosition();
            Vector2 deltaMove = mousePos - _lastMousePosition;
            Position -= deltaMove / Zoom; // Adjust for zoom
            _lastMousePosition = mousePos;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left)
            {
                if (mouseEvent.Pressed)
                {
                    _dragging = true;
                    _lastMousePosition = GetViewport().GetMousePosition();
                }
                else
                {
                    _dragging = false;
                }
            }
            // Handle mouse wheel zoom
            if (mouseEvent.ButtonIndex == MouseButton.WheelUp && mouseEvent.Pressed)
            {
                ZoomCamera(0.025f);
            }
            else if (mouseEvent.ButtonIndex == MouseButton.WheelDown && mouseEvent.Pressed)
            {
                ZoomCamera(-0.025f);
            }
        }
        else if (@event is InputEventMouseMotion && _dragging)
        {
            // Handled in _Process for smoothness
        }
    }

    private void ZoomCamera(float delta)
    {
        // Clamp zoom between 0.1 and 3.0
        float newZoom = Mathf.Clamp(Zoom.X + delta, 0.1f, 3.0f);
        Zoom = new Vector2(newZoom, newZoom);
    }
}
