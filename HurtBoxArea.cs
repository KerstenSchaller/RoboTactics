using Godot;
using System;

public partial class HurtBoxArea : Area2D
{
    [Export]
    public HealthComponent HealthComponent { get; set; }

    private HealthComponent _healthComponent;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (HealthComponent != null )
        {
            _healthComponent = HealthComponent;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void TakeDamage(int amount)
    {
        if (_healthComponent != null)
        {
            _healthComponent.TakeDamage(amount);
        }
        else
        {
            GD.PrintErr("HealthComponent not assigned to HitBoxArea.");
        }
    }
}
