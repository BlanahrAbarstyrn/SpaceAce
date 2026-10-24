using Godot;
using System;
using SpaceAce.Globals;
using SpaceAce.Scenes.bullet;

namespace SpaceAce.Scenes.player;
public partial class Player : Area2D
{
	[Export] private float _speed = 250.0f;
	[Export] private PackedScene _playerLaser;
	[Export] private Marker2D _shootPoint;

	private const float Margin = 64.0f;
	public const string GroupName = "player";
	
	private Vector2 _upperLeftCorner;
	private Vector2 _lowerRightCorner;

	public override void _EnterTree()
	{
		AddToGroup(GroupName);
	}	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetLimits();
		AreaEntered += OnAreaEntered;
	}

	private void SetLimits()
	{
		Rect2 vpr = GetViewportRect();
		_upperLeftCorner = new Vector2(Margin, Margin);
		_lowerRightCorner = new Vector2(vpr.Size.X - Margin, vpr.Size.Y - Margin);
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Position += GetInput() * _speed * (float)delta;
		Position = Position.Clamp(_upperLeftCorner, _lowerRightCorner);
		if (Input.IsActionJustPressed("shoot"))
		{
			SignalManager.EmitOnSpawnPoolObject(_shootPoint.GlobalPosition,  _playerLaser);
		}
	}

	private Vector2 GetInput()
	{
		var v = new Vector2(
			Input.GetAxis("left", "right"),
			Input.GetAxis("up", "down")
			);
		return v.Normalized(); // .Normalized() prevents speeding up in a diagonal direction
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area is Bullet bullet)
		{
			SignalManager.EmitOnPlayerTakeDamage(bullet.Damage);
		}
	}
}
