using Godot;
using System;

public partial class TriggerArea : Area3D
{
	[Signal]
	public delegate void TriggeredEventHandler(Player player);
	private bool _wasTriggered;
	
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node3D body)
	{
		if (_wasTriggered) {
			return;
		}
		if (body is Player player)
		{
			_wasTriggered = true;
			GD.Print("Player entered the area");
			EmitSignal(SignalName.Triggered, player);
			SetDeferred(Area3D.PropertyName.Monitoring, false);
		}
	}
}
