using Godot;
using System;
using MagicTechnology;
using MagicTechnology.Interfaces;
using MagicTechology;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	public const float MouseSensitivity = 0.002f; 
	
	public bool isOnLadder = false;
	
	private Node3D _head;
	private Camera3D _camera;
	private RayCast3D _rayCast;
	
	public void CheckForInteract()
	{
		if (_rayCast.IsColliding()) 
		{
			GD.Print("Ray is collidung!");
			GodotObject collider = _rayCast.GetCollider();
			
			if (collider is IInteractable interactable)
			{
				interactable.Interact(this);
			}
			else
			{
				GD.PrintErr("Объект под лучом не поддерживает взаимодействие (не реализует IInteractable)");
			}
		}
	}
	
	public override void _Ready() 
	{
		base._Ready();
		
		_head = GetNode<Node3D>("Head");
		_camera = GetNode<Camera3D>("Head/Camera3D");
		_rayCast = GetNode<RayCast3D>("Head/Camera3D/RayCast3D");
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouseButton)
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}
		if (@event.IsActionPressed("ActMenuCancel")) 
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		else if (@event.IsActionPressed("ActInteract"))
		{
			GD.Print("Check interact");
			CheckForInteract();
		}
		else if (@event.IsActionPressed("ActShoot"))
		{
#if false
			//test
			var bulletScene = GD.Load<PackedScene>("res://scenes/environments/Bullet.tscn");
			var b = bulletScene.Instantiate<Bullet>();
			
			b.Speed = 10f;
			Camera3D camera = GetNode<Camera3D>("Head/Camera3D");
			b.Direction = -camera.GlobalTransform.Basis.Z;
			b.Position = Position;
			GetTree().CurrentScene.AddChild(b);
			//end test
#endif
			SpellManager.GetInstance().CastSpell(this, "TestSpell");

		}
		
		if (Input.MouseMode == Input.MouseModeEnum.Captured
			&& @event is InputEventMouseMotion eventMouseMotion)
		{
			_head.RotateY(-eventMouseMotion.Relative.X * MouseSensitivity);
			_camera.RotateX(-eventMouseMotion.Relative.Y * MouseSensitivity);
			Vector3 currentRotation = _camera.Rotation;
			float limitedX = Mathf.Clamp(
				(currentRotation.X),
				-Mathf.Pi / 2f,  
				Mathf.Pi / 2f  
			);
			_camera.Rotation = new Vector3(limitedX, currentRotation.Y, currentRotation.Z);
		}
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		if (isOnLadder) 
		{
			if (Input.IsActionPressed("ActJump")) 
			{
				velocity.Y = Speed;
			}
			else if (Input.IsActionPressed("ActCrouch") && !IsOnFloor())
			{
				velocity.Y = -Speed;
			}
			else 
			{
				velocity.Y = 0;
			}
		}
		else 
		{
			// Add the gravity.
			if (!IsOnFloor())
			{
				velocity += GetGravity() * (float)delta;
			}
			// Handle Jump.
			if (Input.IsActionPressed("ActJump") && IsOnFloor())
			{
				velocity.Y = JumpVelocity;
			}
		}
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("ActMoveLeft", "ActMoveRight", "ActMoveForward", "ActMoveBackward");
		Vector3 direction = (_head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
