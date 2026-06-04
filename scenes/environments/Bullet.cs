using Godot;
using System;

public partial class Bullet : Node3D
{
	public float Speed;
	public Vector3 Direction;
	private int timeBeforeDie;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Direction = Direction.Normalized();
		timeBeforeDie = 360;
		GD.Print("Bullet is ready");
		GD.Print("Bullet direction: ", Direction);
		GD.Print("Bullet position: ", Position);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += Direction * Speed * (float)delta;
		--timeBeforeDie;
		if (timeBeforeDie == 0) {
			QueueFree();
		}
		//GD.Print("Now Bullet pos: ", newPosition);
	}
}
