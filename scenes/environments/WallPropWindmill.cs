using Godot;
using System;

public partial class WallPropWindmill : MeshInstance3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Rotation = new Vector3(
			Rotation.X + ((Mathf.Pi / 6f) * (float)delta),
			Rotation.Y,
			Rotation.Z
		);
	}
}
