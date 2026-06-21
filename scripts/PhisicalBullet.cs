using Godot;
using System;

public partial class PhisicalBullet : CharacterBody3D
{
	public float Speed;
	public float Damage;
	public Vector3 Direction;
	private double timeBeforeDie;

	public override void _Ready()
	{
		Direction = Direction.Normalized();
		timeBeforeDie = 20;
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
			QueueFree();
			return;
		}
		timeBeforeDie -= delta;
		if (timeBeforeDie < 0)
			QueueFree();
	}
}
