using Godot;
using System;

public partial class RightHand : MeshInstance3D
{
	private const float _frequency = 5.0f;
	private const float _xAmplitude = 0.04f;
	private const float _yAmplitude = 0.01f;
	private const float _zAmplitude = 0.05f;
	private const float _speedOfReturn = 0.04f;
	
	private Vector3 _startPosition;
	private CharacterBody3D _player;
	private float _time = 0.0f;
	
	public override void _Ready()
	{
		_player = GetOwner<CharacterBody3D>();
		_startPosition = Position;
		if (_player == null)
			GD.PrintErr("У Hand нет соответствующего CharacterBody3D");
	}

	public override void _Process(double delta)
	{
		if (_player == null) return;
		if (_player.IsOnFloor() && _player.Velocity.Length() > 0.1f)
		{
			_time += (float)delta;
			Vector3 newPos = _startPosition;
			newPos.Y += (Mathf.Cos(2*_time * _frequency) - 1) * _yAmplitude;
			newPos.X += Mathf.Sin(_time * _frequency) * _xAmplitude;
			Vector3 newRot = Rotation;
			newRot.Z = -Mathf.Sin(_time * _frequency) * _zAmplitude;
			Position = newPos;
			Rotation = newRot;
		}
		else
		{
			_time = 0;
			Position = Position.Lerp(_startPosition, (float)delta * 5.0f);
			Rotation = new Vector3(
				Mathf.LerpAngle(Rotation.X, 0, (float)delta * 5.0f),
				Mathf.LerpAngle(Rotation.Y, 0, (float)delta * 5.0f),
				Mathf.LerpAngle(Rotation.Z, 0, (float)delta * 5.0f)
			);
		}
	}
}
