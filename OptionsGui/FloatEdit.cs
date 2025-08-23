using Godot;
using System;
using PersistentParameter;

public partial class FloatEdit : HFlowContainer
{
    public void Init(FloatParameter parameter)
    {
        _parameter = parameter;
        if (_parameter != null)
        {
            _min = _parameter.Min;
            _max = _parameter.Max;
            if (_label != null) _label.Text = _parameter.Name;
            if (_valueLineEdit != null) _valueLineEdit.Text = _parameter.Value.ToString("0.###");
            if (_hSlider != null)
            {
                _hSlider.MinValue = _min;
                _hSlider.MaxValue = _max;
                _hSlider.Value = _parameter.Value;
            }
        }
    }
    [Export]
    public string ParameterName { get; set; } = "XXXX";

    private Label _label;
    private LineEdit _valueLineEdit;
    private HSlider _hSlider;

    private FloatParameter _parameter;
    private float _min = 0f;
    private float _max = 1f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _label = GetNode<Label>("Label");
        _valueLineEdit = GetNode<LineEdit>("ValueLineEdit");
        _hSlider = GetNode<HSlider>("HSlider");

        _label.Text = "No Param Available";

        _hSlider.ValueChanged += OnSliderValueChanged;
        _hSlider.SizeFlagsHorizontal = SizeFlags.ExpandFill; 

        _valueLineEdit.TextChanged += OnLineEditTextChanged;
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
