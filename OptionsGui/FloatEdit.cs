using Godot;
using System;
using PersistentParameter;

public partial class FloatEdit : HFlowContainer
{
    public void Init(FloatParameter parameter)
    {
        _parameter = parameter;

        _label = GetNode<Label>("Label");
        _valueLineEdit = GetNode<LineEdit>("ValueLineEdit");
        _hSlider = GetNode<HSlider>("HSlider");

        if (_parameter != null)
        {
            _min = _parameter.Min;
            _max = _parameter.Max;

            _label.Text = _parameter.Name;
            _valueLineEdit.Text = _parameter.Value.ToString("0.###");

            _hSlider.MinValue = _min;
            _hSlider.MaxValue = _max;
            _hSlider.Value = _parameter.Value;
            _hSlider.Step = StepSize;
        }
        else
        {
            _label.Text = "No Param Available";
            _valueLineEdit.Text = "";
            _hSlider.Value = _min;
        }

        _hSlider.ValueChanged += OnSliderValueChanged;
        _hSlider.SizeFlagsHorizontal = SizeFlags.ExpandFill;

        _valueLineEdit.TextChanged += OnLineEditTextChanged;
    }

    [Export]
    public string ParameterName { get; set; } = "XXXX";

    [Export]
    public float StepSize { get; set; } = 0.1f;

    private Label _label;
    private LineEdit _valueLineEdit;
    private HSlider _hSlider;

    private FloatParameter _parameter;
    private float _min = 0f;
    private float _max = 1f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void OnSliderValueChanged(double newValue)
    {
        float val = (float)newValue;
        _parameter.Value = val;
        _valueLineEdit.Text = val.ToString("0.###");
    }

    private void OnLineEditTextChanged(string newText)
    {
        if (float.TryParse(newText, out float newValue))
        {
            newValue = Mathf.Clamp(newValue, _min, _max);
            _parameter.Value = newValue;
            _hSlider.Value = newValue;
        }
    }
}
