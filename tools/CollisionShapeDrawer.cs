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
            DrawPolygonShape(polygon.Polygon, polygon.GetGlobalTransform(), new Color(1, 1, 0, 0.2f), new Color(1, 1, 0, 0.8f));
        }
        foreach (var (shape, owner) in GetAllConvexPolygonsRecursive(GetParent()))
        {
            DrawPolygonShape(shape.Points, owner.GetGlobalTransform(), new Color(0, 1, 1, 0.2f), new Color(0, 1, 1, 0.8f));
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

    // Returns tuples of (ConvexPolygonShape2D, owner Node2D)
    private System.Collections.Generic.IEnumerable<(ConvexPolygonShape2D, Node2D)> GetAllConvexPolygonsRecursive(Node node)
    {
        if (node is CollisionShape2D cs && cs.Shape is ConvexPolygonShape2D convex && node is Node2D node2d)
        {
            yield return (convex, node2d);
        }
        foreach (Node child in node.GetChildren())
        {
            foreach (var c in GetAllConvexPolygonsRecursive(child))
                yield return c;
        }
    }
    private void DrawPolygonShape(Vector2[] points, Transform2D transform, Color fillColor, Color outlineColor)
    {
        if (points.Length >= 3)
        {
            Vector2[] transformedPoints = new Vector2[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                transformedPoints[i] = this.ToLocal(transform * points[i]);
            }
            DrawPolygon(transformedPoints, new Color[] { fillColor });
            DrawPolyline(transformedPoints, outlineColor, 2, true);
        }
    }
}
