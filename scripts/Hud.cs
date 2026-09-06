using Godot;
using System;

public partial class Hud : CanvasLayer
{
	[Export] private TextureProgressBar _flyingBar;
	private int _segmentsCount = 15;

	public override void _Ready()
	{
		_flyingBar.MinValue = 0.0;
		_flyingBar.MaxValue = 1.0;
		_flyingBar.Value = 1.0;
	}

	public void OnFlyingChanged(float value)
	{
		float segmentValue = Mathf.Floor(value * _segmentsCount) / _segmentsCount;
		_flyingBar.Value = segmentValue;
	}

}
