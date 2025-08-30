using Godot;
using System;

    [Tool]
    public partial class Main : Node2D
    {
        private PackedScene _agentScene;
    // ...existing code...

    /// <summary>
    /// Adds StaticBody2D nodes with SegmentShape2D collision shapes along the borders of a Sprite2D child.
    /// </summary>
    /// <param name="spriteNodeName">The name of the Sprite2D child node.</param>
    void AddStaticBodiesAlongSpriteBorders(string spriteNodeName)
    {
        var sprite = GetNodeOrNull<Sprite2D>(spriteNodeName);
        if (sprite != null && sprite.Texture != null)
        {
            Vector2 texSize = sprite.Texture.GetSize();
            Vector2 scale = sprite.Scale;
            Transform2D spriteTransform = sprite.GetGlobalTransform();

            // Apply scale to texture size
            Vector2 scaledSize = texSize * scale;

            Vector2[] corners = new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(scaledSize.X, 0),
                new Vector2(scaledSize.X, scaledSize.Y),
                new Vector2(0, scaledSize.Y)
            };
            // Offset to center, then transform to global
            Vector2 offset = scaledSize / 2.0f;
            for (int i = 0; i < corners.Length; i++)
                corners[i] = spriteTransform * (corners[i] - offset);

            for (int i = 0; i < corners.Length; i++)
            {
                Vector2 a = corners[i];
                Vector2 b = corners[(i + 1) % corners.Length];
                var staticBody = new StaticBody2D();
                var collisionShape = new CollisionShape2D();
                var segment = new SegmentShape2D
                {
                    A = a - staticBody.GlobalPosition,
                    B = b - staticBody.GlobalPosition
                };
                collisionShape.Shape = segment;
                staticBody.AddChild(collisionShape);
                AddChild(staticBody);
            }
        }
        else
        {
            GD.PrintErr($"Sprite2D child '{spriteNodeName}' or its texture not found.");
        }
    }

    public override void _Ready()
    {
        // Load the AutonomousAgentScene.tscn once
        _agentScene = GD.Load<PackedScene>("res://AutonomousAgentScene.tscn");
        if (_agentScene == null)
        {
            GD.PrintErr("Failed to load AutonomousAgentScene.tscn");
            return;
        }

        for (int i = 0; i < 0; i++)
        {
            InstantiateAgent(i);
        }
        //AddStaticBodiesAlongSpriteBorders("Background");
    }

    private void InstantiateAgent(int index)
    {
        var agentInstance = _agentScene.Instantiate();
        var backgroundSprite = GetNodeOrNull<Sprite2D>("Background");
        if (agentInstance is Node2D node2D && backgroundSprite != null && backgroundSprite.Texture != null)
        {
            // Get background sprite's global transform and size
            Vector2 texSize = backgroundSprite.Texture.GetSize()/5;
            Vector2 scale = backgroundSprite.Scale;
            Vector2 scaledSize = texSize * scale;
            Transform2D spriteTransform = backgroundSprite.GetGlobalTransform();

            // Calculate the four corners in global coordinates
            Vector2 offset = scaledSize / 2.0f;
            Vector2 topLeft = spriteTransform * (new Vector2(0, 0) - offset);
            Vector2 bottomRight = spriteTransform * (new Vector2(scaledSize.X, scaledSize.Y) - offset);

            // Calculate min/max X/Y with 100px margin
            float minX = Mathf.Min(topLeft.X, bottomRight.X) + 100;
            float maxX = Mathf.Max(topLeft.X, bottomRight.X) - 100;
            float minY = Mathf.Min(topLeft.Y, bottomRight.Y) + 100;
            float maxY = Mathf.Max(topLeft.Y, bottomRight.Y) - 100;

            var random = new Random();
            float x = (float)(random.NextDouble() * (maxX - minX) + minX);
            float y = (float)(random.NextDouble() * (maxY - minY) + minY);
            node2D.Position = new Vector2(x, y);
        }
        AddChild(agentInstance);
    }




}
