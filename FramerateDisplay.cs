using Godot;
using System;

[Tool]
public partial class FramerateDisplay : Label
{
    public override void _Ready()
    {
        var font = GD.Load<Font>("res://fonts/MedodicaRegular.otf");
        AddThemeFontOverride("font", font);
    }

    public override void _Process(double delta)
    {
        Text = $"FPS: {Engine.GetFramesPerSecond()}";
    }
}
