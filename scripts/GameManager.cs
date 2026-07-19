using Godot;
using System;

public partial class GameManager : Node
{
	public override void _Ready()
	{
		GD.Print("GameManager: Initializing ...");

		// Initialize the spell manager
		//SpellManager.GetInstance().Init();
	}
}
