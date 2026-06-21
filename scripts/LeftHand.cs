using Godot;
using System;

public partial class LeftHand : MeshInstance3D
{
	private const float _climbFrequency = 7.0f;
	private const float _climbYAmplitude = 0.2f;
	private const float _climbZAmplitude = 0.03f;
	private Vector3 _startPosition;
	private Player _player;
	private float _time = 0.0f;
	
	public override void _Ready()
	{
		_player = GetOwner<Player>();
		_startPosition = Position;
		if (_player == null)
			GD.PrintErr("У Hand нет соответствующего Player");
	}

	public override void _Process(double delta)
	{
		if (_player == null) return;
		if (_player.isOnLadder) {
			Visible = true;
			if (Mathf.Abs(_player.Velocity.Y) > 0.1f)
			{
				_time += (float) delta * _climbFrequency;
			}
			AnimateClimbing(Mathf.Pi, (float) delta);
		}
		else {
			Visible = false;
			_time = 0;
			Position = Position.Lerp(_startPosition, (float)delta * 5.0f);
		}
	}
	
	private void AnimateClimbing(float phase, float delta)
	{
		float t = _time + phase;
		Vector3 newPos = _startPosition;
		newPos.Y += Mathf.Sin(t)  * _climbYAmplitude;
		newPos.Z += Mathf.Cos(t) * _climbZAmplitude;
		Position = Position.Lerp(newPos, delta * 5.0f);
	}
}
