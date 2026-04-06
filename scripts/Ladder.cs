using Godot;
using System;
using MagicTechnology.Interfaces;

public partial class Ladder : StaticBody3D, IInteractable
{
	public bool isPlayerIn = false;
	public Vector3 startPosition;
	public Vector3 endPosition;
	
	private Marker3D _startPoint;
	private Marker3D _endPoint;
	
	public override void _Ready()
	{
		_startPoint = GetNode<Marker3D>("StartPoint");
		_endPoint = GetNode<Marker3D>("EndPoint");
		startPosition = _startPoint.GlobalPosition;
		endPosition = _endPoint.GlobalPosition;
	}
	
	public override void _Process(double delta)
	{
	}
	
	private void OnInteractAreaBodyEntered(Node3D body)
	{
		if (body is Player) 
		{
			isPlayerIn = true;
		}
	}
	
	private void OnInteractAreaBodyExiting(Node3D body)
	{
		if (body is Player) 
		{
			isPlayerIn = false;
		}
	}
	
	public void Interact(Player player)
	{
		GD.Print("Player interact with lader");
		if (isPlayerIn)
		{
			player.EnterLadderMode(this);
			GD.Print("Player start climbing");
		}
	}
}
