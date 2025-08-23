using Godot;
using System.Text;
using PersistentParameter;

[Tool]
public partial class ParameterEditorWindow : Window
{
	private VBoxContainer _vbox;
    PackedScene floatEditScene = GD.Load<PackedScene>("res://OptionsGui/FloatEdit.tscn");
    public override void _Ready()
    {
        // Find the VBoxContainer child (assumes it's a direct child, adjust if needed)
        _vbox = GetNodeOrNull<VBoxContainer>("VBoxContainer");
        if (_vbox == null)
        {
            GD.PrintErr("ParameterEditorWindow: VBoxContainer child not found.");
            return;
        }
        ParameterRegistry.ParameterWindow = this;
        Update();
    }

    public void Update()
    {
        foreach (var param in ParameterRegistry.GetAllParameters())
        {
            if (param.ValueType == typeof(float))
            {
                // Cast to FloatParameter (alias for Parameter<float>)
                var floatParam = param as FloatParameter;
                if (floatParam != null)
                {
                    GD.Print($"Creating FloatEdit for parameter: {floatParam.Name}");
                    var floatEdit = floatEditScene.Instantiate<FloatEdit>();
                    _vbox.AddChild(floatEdit);
                    floatEdit.Init(floatParam);
                }

            }
        }
    }
}
