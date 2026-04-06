using Godot;
using System;
using MagicTechnology.Interfaces;

public partial class Player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	public const float MouseSensitivity = 0.002f; 
	
	private bool _isOnLadder = false;
	private Ladder _currentLadder;
	
	[Export]
	private float _climbSpeed = 3.0f;
	[Export]
	private float _pushForce = 2.0f;
	
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
	
	public void EnterLadderMode(Ladder ladder)
	{
		_currentLadder = ladder;
		_isOnLadder = true;
		Velocity = Vector3.Zero;
		GlobalPosition = _currentLadder.startPosition;
		
		GD.Print("Let's climb!");
	}
	
	private void HandleLadderMovement(float delta)
	{
		float verticalInput = Input.GetAxis("ActMoveBackward", "ActMoveForward");
		Vector3 newPosition = GlobalPosition;
		newPosition.Y += verticalInput * _climbSpeed * delta;
		float minY = _currentLadder.startPosition.Y;
		float maxY = _currentLadder.endPosition.Y;
		newPosition.Y = Mathf.Clamp(newPosition.Y, minY, maxY);
		GlobalPosition = newPosition;
		if (Mathf.Abs(newPosition.Y - maxY) < 0.1f)
		{
			ExitLadderMode();
		}
	}
	
	private void ExitLadderMode()
	{
		Vector3 ladderEndPosition = _currentLadder.endPosition;
		Vector3 ladderStartPosition = _currentLadder.startPosition;
		_isOnLadder = false;
		_currentLadder = null;
		Vector3 directionToEnd = (ladderEndPosition - ladderStartPosition).Normalized();
		Vector3 horizontalDirection = new Vector3(directionToEnd.X, 0, directionToEnd.Z).Normalized();
		Velocity = horizontalDirection * _pushForce;
		
		GD.Print("Climb ended");
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
		else if (@event.IsActionPressed("ActMenuCancel")) 
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		else if (@event.IsActionPressed("ActInteract"))
		{
			GD.Print("Check interact");
			CheckForInteract();
		}
		else if (_isOnLadder && @event.IsActionPressed("ActJump"))
		{
			ExitLadderMode();
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
		if (_isOnLadder)
		{
			HandleLadderMovement((float)delta);
			return; 
		}
		
		Vector3 velocity = Velocity;

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
