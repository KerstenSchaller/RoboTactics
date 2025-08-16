using Godot;
using System;

public partial class Vision : Area2D
{
    public Godot.Collections.Array<CharacterBody2D> GetCharacterBodiesInSight()
    {
        var bodies = new Godot.Collections.Array<CharacterBody2D>();
        foreach (var body in GetOverlappingBodies())
        {
            if (body is CharacterBody2D cb)
            {
                // exclude self
                if (cb != (CharacterBody2D)GetParent())
                {
                    bodies.Add(cb);
                    GD.Print($"Overlapping Body: {body.Name}, Type: {body.GetType()}");

                }

             }
        }
        return bodies;
    }



    public override void _Ready()
    {
        // Connect the area entered signal
        this.AreaEntered += _on_area_entered;
        this.AreaExited += _on_area_exited;
    }

    public Godot.Collections.Array<Area2D> GetAllOverlappingAreas()
    {
        var areas = new Godot.Collections.Array<Area2D>();
        foreach (var area in GetOverlappingAreas())
        {
            if (area is Area2D area2D)
                areas.Add(area2D);
        }
        return areas;
    }

    public override void _Process(double delta)
    {
        GetCharacterBodiesInSight();
        int cnt = 0;
        foreach (var overlappingArea in GetOverlappingAreas())
        {
            cnt++;
            //GD.Print($"{cnt}  Overlapping Area: {overlappingArea.Name}, Type: {overlappingArea.GetType()}");
        }
        foreach (var overlappingBody in GetOverlappingBodies())
        {
            //GD.Print($"Overlapping Body: {overlappingBody.Name}, Type: {overlappingBody.GetType()}");
        }
    }

    public void _on_area_entered(Area2D area)
    {
        // You can add additional logic here, such as triggering a behavior or updating the state of the agent.
    }
    
        public void _on_area_exited(Area2D area)
    {
        // You can add additional logic here, such as triggering a behavior or updating the state of the agent.
    }
}
