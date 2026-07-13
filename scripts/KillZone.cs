using Godot;
using System;

public partial class KillZone : Area3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// GD.Print("Area3D is ready");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void OnBodyEntered(Node3D body)
	{
		GD.Print($"<KillZone>: Body entered: {body.Name}");
		
		body.Position = new Vector3(0, 20, 0);
		// Example: Check if the body is a specific type (e.g., Player)
		if (body is Player player)
		{
			GD.Print("<KillZone>: This is player, he entered the area!");
		}
	}
}
