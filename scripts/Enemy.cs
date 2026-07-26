using Godot;
using System;
using System.Collections.Generic;

public partial class Enemy : CharacterBody3D
{
	[Export] private float MaxHealth = 100.0f;
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	private List<MeshInstance3D> _meshes = new();
	private Dictionary<MeshInstance3D, Material> _originalMaterials = new();
	private StandardMaterial3D _damageMaterial;

	private float _currentHealth;
	protected bool _isDead = false;
	private double _flashTimeLeft = 0.0;
	
	public override void _Ready() 
	{
		_currentHealth = MaxHealth;
		FindMeshes(this);
		_damageMaterial = new StandardMaterial3D();
		_damageMaterial.AlbedoColor = Colors.White;
	}
	
	private void FindMeshes(Node node)
	{
		foreach (Node child in node.GetChildren())
		{
			if (child is MeshInstance3D mesh)
			{
				_meshes.Add(mesh);
				_originalMaterials[mesh] = mesh.MaterialOverride;
			}
			FindMeshes(child);
		}
	}
	
	protected virtual void Die() {
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
		foreach (MeshInstance3D mesh in _meshes) {
			if (mesh != null) {
				mesh.MaterialOverride = _damageMaterial;
				_flashTimeLeft = 0.2;
			}
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (_flashTimeLeft > 0.0)
		{
			_flashTimeLeft -= delta;
			if (_flashTimeLeft <= 0.0)
			{
				foreach (MeshInstance3D mesh in _meshes) {
					mesh.MaterialOverride = _originalMaterials[mesh];
				}
			}
		}
		Vector3 velocity = Velocity;
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		Velocity = velocity;
		MoveAndSlide();
	}
}
