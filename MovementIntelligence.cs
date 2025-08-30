using Godot;
using System;

class Printer
{
    FloatParameter message;
    public Printer(FloatParameter message)
    {
        this.message = message;
    }
    public void Print()
    {
        GD.Print("name: " + message.Name + ", value: " + message.Value);
    }
}

public partial class MovementIntelligence : Node
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {


        FloatParameter fp = PersistentParameter.ParameterRegistry.GetFloatParameter("xyx.xxx", 1.0f, 0, 10);

        Printer printer = new Printer(fp);
        printer.Print();
        fp.Value = 5.0f;

        Printer printer2 = new Printer(fp);
        printer2.Print();



        fp = new PersistentParameter.Parameter<float>("yyy.xxx", 1.0f, 0, 10);

        printer.Print();
        printer2.Print();
    }



    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
