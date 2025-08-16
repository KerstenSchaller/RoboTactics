using Godot;
using System;

public partial class HealthComponent : Node
{
    [Export]
    public int MaxHealth { get; set; } = 100;

    public int CurrentHealth { get; private set; }


    public override void _Ready()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || CurrentHealth <= 0)
            return;

        CurrentHealth -= amount;
        //GD.Print($"HealthComponent: Current Health = {CurrentHealth}");
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
        }
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || CurrentHealth <= 0)
            return;

        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
    }
}