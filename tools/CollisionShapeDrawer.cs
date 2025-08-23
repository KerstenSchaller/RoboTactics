using Godot;
using System;

public partial class CollisionShapeDrawer : Node2D
{
    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    public override void _Draw()
    {
        foreach (var polygon in GetAllCollisionPolygonsRecursive(GetParent()))
        {
            DrawCollisionShape(polygon, polygon.GetGlobalTransform());
        }
    }



    private System.Collections.Generic.IEnumerable<CollisionPolygon2D> GetAllCollisionPolygonsRecursive(Node node)
    {
        if (node is CollisionPolygon2D polygon)
        {
            yield return polygon;
        }
        foreach (Node child in node.GetChildren())
        {
            foreach (var p in GetAllCollisionPolygonsRecursive(child))
                yield return p;
        }
    }

    private void DrawCollisionShape(CollisionPolygon2D shape, Transform2D transform)
    {
        var points = shape.Polygon;
        if (points.Length >= 3)
        {
            Vector2[] transformedPoints = new Vector2[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                // Transform the point from the polygon's local space to global, then to this node's local space
                transformedPoints[i] = this.ToLocal(transform * points[i]);
            }
            DrawPolygon(transformedPoints, new Color[] { new Color(1, 1, 0, 0.2f) });
            DrawPolyline(transformedPoints, new Color(1, 1, 0, 0.8f), 2, true);
        }
        // Only drawing PolygonShape2D shapes
    }
}
