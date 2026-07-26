using Godot;
using System;

public partial class Skeleton : Enemy
{
	private string _idleAnimationName = "skeleton-skeleton|idle";
	private string _deathAnimationName = "skeleton-skeleton|spawn";
	private string _runAnimationName = "skeleton-skeleton|run";
	private AnimationPlayer _animationPlayer;
	private Vector3 _runDirection;
	bool _isRunning = false; // state
	
	public override void _Ready()
	{
		base._Ready();
		_animationPlayer = GetNode<AnimationPlayer>("Node3D/skeleton/AnimationPlayer");
		if (_animationPlayer == null) {
			GD.Print($"AnimationPlayer not found");
			return;
		}
		_animationPlayer.AnimationFinished += OnAnimationFinished;
		Animation animation = _animationPlayer.GetAnimation(_idleAnimationName);
		if (animation != null)
		{
			animation.LoopMode = Animation.LoopModeEnum.Linear;
		}
		Animation deathAnimation = _animationPlayer.GetAnimation(_deathAnimationName);
		if (deathAnimation != null)
		{
			deathAnimation.LoopMode = Animation.LoopModeEnum.None;
		}
		_animationPlayer.Play(_idleAnimationName);
		Animation runAnimation = _animationPlayer.GetAnimation(_runAnimationName);
		if (runAnimation != null)
		{
			runAnimation.LoopMode = Animation.LoopModeEnum.Linear;
		}
	}

	protected override void Die() {
		_isDead = true;
		GD.Print($"{Name} умер");
		if (_animationPlayer != null && _animationPlayer.HasAnimation(_deathAnimationName)) {
			_animationPlayer.PlayBackwards(_deathAnimationName);
		}
		else {
			QueueFree();
		}
	}
	
	public void FacePlayer(Player player) {
		if (_isDead) return;
		Vector3 targetPos = player.GlobalPosition;
		Vector3 lookPos = new Vector3(targetPos.X, GlobalPosition.Y, targetPos.Z);
		LookAt(lookPos, Vector3.Up);
	}
	
	public void StartRunning()
	{
		if (_isDead) return;
		_isRunning = true;
		if (_animationPlayer != null && _animationPlayer.HasAnimation(_runAnimationName))
		{
			_animationPlayer.Play(_runAnimationName);
		}
		float randomAngle = (float)GD.RandRange(0, Mathf.Pi * 2);
		_runDirection = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)).Normalized();
		LookAt(GlobalPosition + _runDirection, Vector3.Up);
	}
	
	public void StopRunning()
	{
		if (_isDead) return;
		_isRunning = false;
		Velocity = Vector3.Zero;
		QueueFree();
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		if (_isRunning)
		{
			velocity.X = _runDirection.X * Speed;
			velocity.Z = _runDirection.Z * Speed;
		}
		else
		{
			velocity.X = 0;
			velocity.Z = 0;
		}
		Velocity = velocity;
		base._PhysicsProcess(delta);
	}
	
	private void OnAnimationFinished(StringName animationName) {
		if (_isDead && animationName == _deathAnimationName) {
			QueueFree();
		}
	}
}
