using Godot;
using System.Collections.Generic;

public partial class BoundaryArea : Area2D
{
    private Rect2 boundaryRect;
    private List<CharacterBody2D> characters = new List<CharacterBody2D>();

    public override void _Ready()
    {
        // Find the boundary from the CollisionShape2D child
        var collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        if (collisionShape.Shape is RectangleShape2D rectShape)
        {
            var extents = rectShape.Size / 2.0f;
            boundaryRect = new Rect2(
                GlobalPosition - extents,
                rectShape.Size
            );
        }
        else
        {
            GD.PrintErr("BoundaryArea: CollisionShape2D must use RectangleShape2D.");
        }


    }


    public override void _Process(double delta)
    {
        foreach (var cb in GetParent().GetChildren() )
        {
            if (cb is CharacterBody2D character)
            {

                Vector2 pos = character.GlobalPosition;
                bool wrapped = false;

                if (pos.X < boundaryRect.Position.X)
                {
                    pos.X = boundaryRect.Position.X + boundaryRect.Size.X;
                    wrapped = true;
                }
                else if (pos.X > boundaryRect.Position.X + boundaryRect.Size.X)
                {
                    pos.X = boundaryRect.Position.X;
                    wrapped = true;
                }

                if (pos.Y < boundaryRect.Position.Y)
                {
                    pos.Y = boundaryRect.Position.Y + boundaryRect.Size.Y;
                    wrapped = true;
                }
                else if (pos.Y > boundaryRect.Position.Y + boundaryRect.Size.Y)
                {
                    pos.Y = boundaryRect.Position.Y;
                    wrapped = true;
                }
            
                if (wrapped)
                        character.GlobalPosition = pos;
            }

        }
    }
}
