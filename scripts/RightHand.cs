using Godot;
using System;

public partial class RightHand : MeshInstance3D
{
	private const float _frequency = 5.0f;
	private const float _xAmplitude = 0.04f;
	private const float _yAmplitude = 0.01f;
	private const float _zAmplitude = 0.05f;
	private const float _climbFrequency = 7.0f;
	private const float _climbYAmplitude = 0.2f;
	private const float _climbZAmplitude = 0.03f;
	private Vector3 _startPosition;
	private Vector3 _startRotation;
	private Player _player;
	private float _time = 0.0f;
	
	public override void _Ready()
	{
		_player = GetOwner<Player>();
		_startPosition = Position;
		_startRotation = Rotation;
		if (_player == null)
			GD.PrintErr("У Hand нет соответствующего Player");
	}
	
	private void AnimateClimbing(float phase, float delta)
	{
		float t = _time + phase;
		Vector3 newPos = _startPosition;
		newPos.Y += Mathf.Sin(t)  * _climbYAmplitude;
		newPos.Z += Mathf.Cos(t) * _climbZAmplitude;
		Position = Position.Lerp(newPos, delta * 5.0f);
		Rotation = new Vector3(
			Mathf.LerpAngle(Rotation.X, _startRotation.X, delta * 5.0f),
			Mathf.LerpAngle(Rotation.Y, _startRotation.Y, delta * 5.0f),
			Mathf.LerpAngle(Rotation.Z, _startRotation.Z, delta * 5.0f)
		);
	}

	public override void _Process(double delta)
	{
		if (_player == null) return;
		if (_player.isOnLadder)
		{
			if (Mathf.Abs(_player.Velocity.Y) > 0.1f)
				_time += (float) delta * _climbFrequency;
			AnimateClimbing(0.0f, (float) delta);
		}
		else if (_player.IsOnFloor() && _player.Velocity.Length() > 0.1f)
		{
			_time += (float)delta * _frequency;
			Vector3 newPos = _startPosition;
			newPos.Y += (Mathf.Cos(2*_time) - 1) * _yAmplitude;
			newPos.X += Mathf.Sin(_time) * _xAmplitude;
			Vector3 newRot = _startRotation;
			newRot.Z = -Mathf.Sin(_time) * _zAmplitude;
			Position = newPos;
			Rotation = newRot;
		}
		else
		{
			_time = 0;
			Position = Position.Lerp(_startPosition, (float)delta * 5.0f);
			Rotation = new Vector3(
				Mathf.LerpAngle(Rotation.X, _startRotation.X, (float)delta * 5.0f),
				Mathf.LerpAngle(Rotation.Y, _startRotation.Y, (float)delta * 5.0f),
				Mathf.LerpAngle(Rotation.Z, _startRotation.Z, (float)delta * 5.0f)
			);
		}
	}
}
