using Godot;
using System;

[Tool]
public partial class StaticBody2d : StaticBody2D
{
    [Export(PropertyHint.Range, "1,30,1.0f")]
    public float BoundarySize { get; set; } = 1; // Controls the width of the rectangle

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CreateBoundaryRectangles();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        QueueRedraw();
    }

    public override void _Draw()
    {
        float width = 1 * 800;
        float height = width * 9f / 16f;
        float thickness = 20f;

        // Rectangle corners
        Vector2 topLeft = new Vector2(-width / 2f, -height / 2f);
        Vector2 topRight = new Vector2(width / 2f, -height / 2f);
        Vector2 bottomRight = new Vector2(width / 2f, height / 2f);
        Vector2 bottomLeft = new Vector2(-width / 2f, height / 2f);

        // Draw left rectangle
        Vector2[] leftPoly = new Vector2[] {
            new Vector2(topLeft.X - thickness, topLeft.Y - thickness),
            new Vector2(topLeft.X, topLeft.Y - thickness),
            new Vector2(bottomLeft.X, bottomLeft.Y + thickness),
            new Vector2(bottomLeft.X - thickness, bottomLeft.Y + thickness)
        };
        DrawPolygon(leftPoly, new Color[] { Colors.Red, Colors.Red, Colors.Red, Colors.Red });

        // Draw right rectangle
        Vector2[] rightPoly = new Vector2[] {
            new Vector2(topRight.X, topRight.Y - thickness),
            new Vector2(topRight.X + thickness, topRight.Y - thickness),
            new Vector2(bottomRight.X + thickness, bottomRight.Y + thickness),
            new Vector2(bottomRight.X, bottomRight.Y + thickness)
        };
        DrawPolygon(rightPoly, new Color[] { Colors.Green, Colors.Green, Colors.Green, Colors.Green });

        // Draw top rectangle
        Vector2[] topPoly = new Vector2[] {
            new Vector2(topLeft.X - thickness, topLeft.Y - thickness),
            new Vector2(topRight.X + thickness, topRight.Y - thickness),
            new Vector2(topRight.X, topRight.Y),
            new Vector2(topLeft.X, topLeft.Y)
        };
        DrawPolygon(topPoly, new Color[] { Colors.Blue, Colors.Blue, Colors.Blue, Colors.Blue });

        // Draw bottom rectangle
        Vector2[] bottomPoly = new Vector2[] {
            new Vector2(bottomLeft.X - thickness, bottomLeft.Y + thickness),
            new Vector2(bottomRight.X + thickness, bottomRight.Y + thickness),
            new Vector2(bottomRight.X, bottomRight.Y),
            new Vector2(bottomLeft.X, bottomLeft.Y)
        };
        DrawPolygon(bottomPoly, new Color[] { Colors.Yellow, Colors.Yellow, Colors.Yellow, Colors.Yellow });
    }

    private void CreateBoundaryRectangles()
    {
        float width = 1 * 800;
        float height = width * 9f / 16f;
        float thickness = 20f;

        // Update existing CollisionPolygon2D children named "L", "R", "U", "D"
        // L (Left)
        var leftPoly = FindPolygonChild("L");
        if (leftPoly != null)
        {
            leftPoly.SetDeferred("Polygon", new Vector2[]
            {
                new Vector2(-thickness / 2f, -height / 2f - thickness),
                new Vector2(thickness / 2f, -height / 2f - thickness),
                new Vector2(thickness / 2f, height / 2f + thickness),
                new Vector2(-thickness / 2f, height / 2f + thickness),
                new Vector2(-thickness / 2f, -height / 2f - thickness)
            });
            if (leftPoly.GetParent() is Node2D leftNode)
                leftNode.Position = new Vector2(-width / 2f - thickness / 2f, 0);
        }

        // R (Right)
        var rightPoly = FindPolygonChild("R");
        if (rightPoly != null)
        {
            rightPoly.SetDeferred("Polygon", new Vector2[] 
            {
                new Vector2(-thickness / 2f, -height / 2f - thickness),
                new Vector2(thickness / 2f, -height / 2f - thickness),
                new Vector2(thickness / 2f, height / 2f + thickness),
                new Vector2(-thickness / 2f, height / 2f + thickness),
                new Vector2(-thickness / 2f, -height / 2f - thickness)
            });
            if (rightPoly.GetParent() is Node2D rightNode)
                rightNode.Position = new Vector2(width / 2f + thickness / 2f, 0);
        }

        // U (Up/Top)
        var topPoly = FindPolygonChild("U");
        if (topPoly != null)
        {
            topPoly.SetDeferred("Polygon", new Vector2[] 
            {
                new Vector2(-width / 2f - thickness, -thickness / 2f),
                new Vector2(width / 2f + thickness, -thickness / 2f),
                new Vector2(width / 2f + thickness, thickness / 2f),
                new Vector2(-width / 2f - thickness, thickness / 2f),
                new Vector2(-width / 2f - thickness, -thickness / 2f)
            });
            if (topPoly.GetParent() is Node2D topNode)
                topNode.Position = new Vector2(0, -height / 2f - thickness / 2f);
        }

        // D (Down/Bottom)
        var bottomPoly = FindPolygonChild("D");
        if (bottomPoly != null)
        {
            bottomPoly.SetDeferred("Polygon", new Vector2[] 
            {
                new Vector2(-width / 2f - thickness, -thickness / 2f),
                new Vector2(width / 2f + thickness, -thickness / 2f),
                new Vector2(width / 2f + thickness, thickness / 2f),
                new Vector2(-width / 2f - thickness, thickness / 2f),
                new Vector2(-width / 2f - thickness, -thickness / 2f)
            });
            if (bottomPoly.GetParent() is Node2D bottomNode)
                bottomNode.Position = new Vector2(0, height / 2f + thickness / 2f);
        }

        // Request redraw
        QueueRedraw();
    }

    // Helper to find a CollisionPolygon2D child by name
    private CollisionPolygon2D FindPolygonChild(string name)
    {
        var node = GetNodeOrNull<Node2D>(name);
        if (node == null) return null;
        foreach (var child in node.GetChildren())
        {
            if (child is CollisionPolygon2D poly)
                return poly;
        }
        return null;
    }
}
