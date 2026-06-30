using Godot;
using System;

public partial class FireImpact : Node3D
{
	private Node3D _fireCast;
	public override void _Ready()
	{
		_fireCast = GetNode<Node3D>("VFXFireCast");
		_fireCast.Connect("finished", Callable.From(OnCastFinished));
		_fireCast.Set("one_shot", true);
		_fireCast.Set("autoplay", false);
		_fireCast.Call("play");
	}

	private void OnCastFinished()
	{
		QueueFree();
	}
}
