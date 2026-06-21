using Godot;
using System;

public partial class Enemy : CharacterBody3D
{
	[Export] private float MaxHealth = 100.0f;
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	private MeshInstance3D _mesh;
	private Material _originalMaterial;
	private StandardMaterial3D _damageMaterial;

	private float _currentHealth;
	private bool _isDead = false;
	private double _flashTimeLeft = 0.0;
	
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
		if (_mesh != null) {
			_mesh.MaterialOverride = _damageMaterial;
			_flashTimeLeft = 0.2;
		}
	}
	
	public override void _Ready() 
	{
		_currentHealth = MaxHealth;
		_mesh = GetNode<MeshInstance3D>("MeshInstance3D");
		_originalMaterial = _mesh.MaterialOverride;
		_damageMaterial = new StandardMaterial3D();
		_damageMaterial.AlbedoColor = Colors.White;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (_flashTimeLeft > 0.0)
		{
			_flashTimeLeft -= delta;
			if (_flashTimeLeft <= 0.0)
			{
				_mesh.MaterialOverride = _originalMaterial;
			}
		}
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
