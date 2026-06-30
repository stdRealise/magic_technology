using Godot;
using System;

public partial class BarrelFragments : Node3D
{
	private double _lifeTime = 10;
	private float _explosionForce = 2.0f;
	private Vector3 _centerPosition;
	
	public override void _Ready()
	{
		_centerPosition = GlobalPosition;
		ApplyExplosion(this);
	}
	
	private void ApplyExplosion(Node node) {
		foreach (Node child in node.GetChildren())
		{
			if (child is RigidBody3D fragment)
			{
				CollisionShape3D collisionShape = fragment.GetNode<CollisionShape3D>("CollisionShape3D");
				Vector3 direction = (collisionShape.GlobalPosition - _centerPosition).Normalized();
				Vector3 impulse = direction * _explosionForce;
				fragment.ApplyCentralImpulse(impulse);
			}
			ApplyExplosion(child);
		}
	}
	

	public override void _PhysicsProcess(double delta)
	{
		_lifeTime -= delta;
		if (_lifeTime < 0)
			QueueFree();
	}
}
