using Godot;
using System;

[Tool]
public partial class ConnectionTool : Node2D
{
    [Export] Node2D firstNode;
    [Export] Node2D secondNode;

    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    public override void _Draw()
    {
        if (firstNode == null || secondNode == null)
            return;

        // Draw the line
        DrawLine(firstNode.GlobalPosition, secondNode.GlobalPosition, Colors.Yellow);

        // Calculate the midpoint
        Vector2 midPoint = (firstNode.GlobalPosition + secondNode.GlobalPosition) / 2;

        // Calculate the distance
        float distance = firstNode.GlobalPosition.DistanceTo(secondNode.GlobalPosition);

        // Draw the distance as text at the midpoint
        var font = GD.Load<Font>("res://fonts/MedodicaRegular.otf");
        int fontSize = 128; // Increase the font size as needed
        DrawString(
            font,
            midPoint,
            distance.ToString("0.##"),
            fontSize: fontSize,
            modulate: Colors.White
        );
    }
}
