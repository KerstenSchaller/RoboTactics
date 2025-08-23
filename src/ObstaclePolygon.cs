using Godot;

public partial class ObstaclePolygon : CollisionPolygon2D
{
    bool initialized = false;
  
    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    public override void _Draw()
    {
        if (Polygon != null && Polygon.Length > 2)
        {
            DrawPolygon(Polygon, new Color[] { Colors.Green });
        }
    }
}
