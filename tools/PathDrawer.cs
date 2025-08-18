using Godot;
using System.Collections.Generic;

[Tool]
public partial class PathDrawer : Node2D
{
    [Export] public bool Enabled { get; set; }
    [Export] public NodePath TargetNode { get; set; }
    [Export] public int MaxLineSegments { get; set; } = 50;
    [Export] public float DistanceThreshold { get; set; } = 10.0f;

    private List<Vector2> _points = new List<Vector2>();
    private Vector2 _lastPosition;

    public override void _Ready()
    {
        if (TargetNode != null && !TargetNode.IsEmpty)
        {
            var node = GetNodeOrNull<Node2D>(TargetNode);
            if (node != null)
            {
                _lastPosition = node.GlobalPosition;
                _points.Add(_lastPosition);
            }
        }
    }

    public override void _Process(double delta)
    {
        if (TargetNode == null || TargetNode.IsEmpty)
            return;

        var node = GetNodeOrNull<Node2D>(TargetNode);
        if (node == null)
            return;

        if (!Enabled)
            return;

        Vector2 currentPos = node.GlobalPosition;

        if (_points.Count == 0 || _points[_points.Count - 1].DistanceTo(currentPos) >= DistanceThreshold)
        {
            _points.Add(currentPos);

            if (_points.Count > MaxLineSegments)
                _points.RemoveAt(0);

            QueueRedraw();
        }
    }

    public override void _Draw()
    {
        if (_points.Count < 2)
            return;

        for (int i = 0; i < _points.Count - 1; i++)
        {
            DrawLine(ToLocal(_points[i]), ToLocal(_points[i + 1]), Colors.Red, 2.0f, true);
        }
    }
}
