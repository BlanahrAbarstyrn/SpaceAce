using Godot;
using System;
using SpaceAce.Classes;
using SpaceAce.Contracts;
using SpaceAce.Globals;
using SpaceAce.Scenes.player;

namespace SpaceAce.Scenes.homing_missile;
public partial class HomingMissile : Node2D, iPool_Item
{
	[Export] private Area2D _hitArea;
	[Export] private float _speed = 50.0f;
	[Export] private float _rotationSpeed = 2.0f;
	[Export] private Timer _invincibleTimer;
	[Export] private PackedScene _explosionScene;
	[Export] private int _points = 25;
	
	private Player _player;
	private bool _isInvincible;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_hitArea.AreaEntered += OnHitAreaEntered;
		_invincibleTimer.Timeout += OnInvincibleTimerTimeout;
		_player = GetTree().GetFirstNodeInGroup(Player.GroupName) as Player;
		if (_player == null)
		{
			GD.PrintErr("HomingMissile: No player found");
		}
		Deactivate();
	}
	
	public override void _ExitTree()
	{
		Input.StopJoyVibration(0);
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		float angleToPlayer = Transform.X.AngleTo(
			GlobalPosition.DirectionTo(_player.GlobalPosition)
		);
		float rotationStep = _rotationSpeed * (float)delta;
		float rotationDirection = Godot.Mathf.Sign(angleToPlayer); 
		float step = Godot.Mathf.Min(Godot.Mathf.Abs(angleToPlayer), rotationStep);
		Rotate(step * rotationDirection);
		Position += Transform.X * _speed * (float)delta;
	}

	public void Activate()
	{
		Rotation = 0.0f;
		Show();
		SetPhysicsProcess(true);
		SpaceUtils.ActivateArea2D(_hitArea, true);
		_invincibleTimer.Start();
		_isInvincible = true;
	}

	public void Deactivate()
	{
		Hide();
		SetPhysicsProcess(false);
		SpaceUtils.ActivateArea2D(_hitArea, false);
		_invincibleTimer.Stop();
		_isInvincible = false;
	}

	private void OnHitAreaEntered(Area2D area)
	{
		if (area is Player)
		{
			Input.StartJoyVibration(0, 1.0f, 1.0f, 1.0f);
			SignalManager.EmitOnSpawnPoolObject(GlobalPosition, _explosionScene);
			Deactivate();
		}
		if (_isInvincible) return;
		SignalManager.EmitOnSpawnPoolObject(GlobalPosition, _explosionScene);
		SignalManager.EmitOnScorePoints(_points);
		Deactivate();
	}

	private void OnInvincibleTimerTimeout()
	{
		_isInvincible = false;
	}
}
