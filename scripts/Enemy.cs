using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
	[Export] private float MaxHealth = 100.0f;
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	private float _currentHealth;
	private bool _isDead = false;

	private void Die() {
		_isDead = true;
		GD.Print($"{Name} умер");
		QueueFree();
	}

	public void TakeDamage(float damage) {
		if (_isDead) return;
		if (_currentHealth <= damage) {
			_currentHealth = 0.0f;
			Die();
			return;
		}
		_currentHealth -= damage;
		GD.Print($"{Name} получил урон. HP: {_currentHealth}");
	}
	
	public override void _Ready() 
	{
		_currentHealth = MaxHealth;	
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
	
		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		velocity.X = 0;
		velocity.Z = 0;
		Velocity = velocity;
		MoveAndSlide();
	}
}
