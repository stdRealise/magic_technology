using Godot;
using System;

public partial class Barrel : StaticBody3D
{
	private Node3D _barrelModel;
	private Node3D _brokenModel;
	private CollisionShape3D _collisionShape;
	private bool _isDestroyed = false;
	
	public override void _Ready()
	{
		_barrelModel = GetNode<Node3D>("BarrelModel");
		_brokenModel = GetNode<Node3D>("BrokenModel");
		_collisionShape = GetNode<CollisionShape3D>("CollisionShape3D");
	}
	
	public void Destroy() {
		if (_isDestroyed) {
			return;
		}
		_isDestroyed = true;
		_barrelModel.Visible = false;
		_brokenModel.Visible = true;
		_collisionShape.Disabled = true;
		SpawnFragments();
	}
	
	private void SpawnFragments()
	{
		var fragmentsScene = GD.Load<PackedScene>("res://scenes/environments/BarrelFragments.tscn");
		var fragments = fragmentsScene.Instantiate<Node3D>();
		GetTree().CurrentScene.AddChild(fragments);
		fragments.GlobalPosition = GlobalPosition;
	}

	public override void _Process(double delta)
	{
	}
}
