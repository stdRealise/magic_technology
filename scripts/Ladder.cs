using Godot;
using System;

public partial class Ladder : StaticBody3D
{	
	public override void _Ready()
	{
		
	}
	
	public override void _Process(double delta)
	{
	}
	
	private void OnInteractAreaBodyEntered(Node3D body)
	{
		if (body is Player player) 
		{
			player.isOnLadder = true;
		}
	}
	
	private void OnInteractAreaBodyExiting(Node3D body)
	{
		if (body is Player player) 
		{
			player.isOnLadder = false;
		}
	}
}
