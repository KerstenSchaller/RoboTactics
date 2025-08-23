
using Godot;
using System;
using PersistentParameter;
// For FloatParameter alias
using static PersistentParameter.ParameterRegistry;

[Tool]
public partial class Vision : Area2D
{
    public Godot.Collections.Array<StaticBody2D> GetStaticBodiesInSight()
    {
        var bodies = new Godot.Collections.Array<StaticBody2D>();
        GD.Print($"Static Bodies in Sight:");
        foreach (var body in GetOverlappingBodies())
        {
            if (body is StaticBody2D sb)
            {
                GD.Print($"Static Body in Sight: {sb.Name}");
                bodies.Add(sb);
            }
        }
        return bodies;
    }
    // Exported parameters for procedural vision shape
    [Export]
    public bool UseProceduralVisionShape { get; set; } = false;
    // Use persistent parameter for vision radius
    public FloatParameter VisionRadius = GetFloatParameter("Vision.VisionRadius", 100f, 0f, 1000f);
    public FloatParameter RearCutoutAngleDeg = GetFloatParameter("Vision.RearCutoutAngleDeg", 30f, 0f, 120f);

    private float _lastVisionRadius = float.NaN;
    private float _lastRearCutoutAngleDeg = float.NaN;

    private CollisionPolygon2D _proceduralPolygon = null;
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
                    //GD.Print($"Overlapping Body: {body.Name}, Type: {body.GetType()}");

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

        if (UseProceduralVisionShape)
        {
            CreateProceduralVisionPolygon();
        }
    }



    private void CreateProceduralVisionPolygon()
    {
        // Remove existing polygon if present
        if (_proceduralPolygon != null && _proceduralPolygon.GetParent() == this)
            RemoveChild(_proceduralPolygon);

        _proceduralPolygon = new CollisionPolygon2D();
        _proceduralPolygon.Name = "ProceduralVisionPolygon";

        // Calculate points for the vision shape
        float radius = VisionRadius;
        float rearAngle = Mathf.DegToRad(RearCutoutAngleDeg);
        float startAngle = rearAngle / 2f + Mathf.Pi; // Rotate 180 degrees
        float endAngle = 2f * Mathf.Pi - rearAngle / 2f + Mathf.Pi; // Rotate 180 degrees
        int segments = 48; // More segments = smoother circle

        Vector2[] points = new Vector2[segments + 2];
        int idx = 0;
        // Add points for the visible arc (front)
        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;
            float angle = Mathf.Lerp(startAngle, endAngle, t);
            points[idx++] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        }
        // Add center point to close the polygon
        points[idx] = Vector2.Zero;

        _proceduralPolygon.Polygon = points;
        AddChild(_proceduralPolygon);
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
        if (false)
        {
            if (GetOverlappingAreas().Count != 0 || GetOverlappingBodies().Count != 0)
            {
                GD.Print("--------------------");
            }
            foreach (var overlappingArea in GetOverlappingAreas())
            {
                GD.Print($"{cnt} {GetParent().Name} detected overlapping Area: {overlappingArea.Name}, Type: {overlappingArea.GetType()}");
                cnt++;
            }
            cnt = 0;
            foreach (var overlappingBody in GetOverlappingBodies())
            {
                GD.Print($"{cnt} {GetParent().Name} detected overlapping Body: {overlappingBody.Name}, Type: {overlappingBody.GetType()}");
                cnt++;
            }
        }
        // Check for parameter changes and recreate polygon if needed
        if (UseProceduralVisionShape)
        {
            float currentRadius = VisionRadius.Value;
            float currentRearAngle = RearCutoutAngleDeg.Value;
            if (!Mathf.IsEqualApprox(currentRadius, _lastVisionRadius) || !Mathf.IsEqualApprox(currentRearAngle, _lastRearCutoutAngleDeg))
            {
                CreateProceduralVisionPolygon();
                _lastVisionRadius = currentRadius;
                _lastRearCutoutAngleDeg = currentRearAngle;
            }
        }
    }

    public void _on_area_entered(Area2D area)
    {
        //GD.Print($"Area Entered: {area.Name}");
    }

    public void _on_area_exited(Area2D area)
    {
        //GD.Print($"Area Exited: {area.Name}");
    }
}
