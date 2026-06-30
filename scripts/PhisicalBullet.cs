using Godot;
using System;

public partial class PhisicalBullet : CharacterBody3D
{
	[Export] public PackedScene ExplosionScene;
	public float Speed;
	public float Damage;
	public Vector3 Direction;
	private double timeBeforeDie; // лучше сделать дальность заклинания

	public override void _Ready()
	{
		Direction = Direction.Normalized();
		timeBeforeDie = 3;
	}

	public override void _PhysicsProcess(double delta)
	{
		Velocity = Direction * Speed;
		KinematicCollision3D collision = MoveAndCollide(Velocity * (float)delta);
		if (collision != null)
		{
			GodotObject collider = collision.GetCollider();
			if (collider is Enemy enemy)
			{
				enemy.TakeDamage(Damage);
			}
			else if (collider is Barrel barrel) 
			{
				barrel.Destroy();
			}
			StartExplosion(collision.GetPosition());
			QueueFree();
			return;
		}
		timeBeforeDie -= delta;
		if (timeBeforeDie < 0)
			QueueFree();
	}
	private void StartExplosion(Vector3 position) {
		Node3D explosion = ExplosionScene.Instantiate<Node3D>();
		GetTree().CurrentScene.AddChild(explosion);
		explosion.GlobalPosition = position;
	}
}
