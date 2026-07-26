using Godot;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class EnemiesGroup : Node3D
{
	private TriggerArea _triggerArea;
	private List<Enemy> _enemies = new();
	
	
	public override void _Ready()
	{
		_triggerArea = GetNode<TriggerArea>("TriggerArea");
		Node enemiesNode = GetNode<Node3D>("Enemies");
		 foreach (Node child in enemiesNode.GetChildren())
		{
			if (child is Enemy enemy)
			{
				_enemies.Add(enemy);
			}
		}
		_triggerArea.Triggered += ActivateEnemies;
	}
	
	private async void ActivateEnemies(Player player)
	{
		foreach (Skeleton skeleton in _enemies)
		{
			skeleton.FacePlayer(player);
		}
		await ToSignal(GetTree().CreateTimer(3.0),
			SceneTreeTimer.SignalName.Timeout);
		foreach (Skeleton skeleton in _enemies)
		{
			skeleton.StartRunning();
		}
		await ToSignal(GetTree().CreateTimer(5.0),
			SceneTreeTimer.SignalName.Timeout);
		foreach (Skeleton skeleton in _enemies)
		{
			skeleton.StopRunning();
		}
	}	
}
