using Godot;
using System;
using SpaceAce.Classes;
using SpaceAce.Globals;

namespace SpaceAce.Scenes.power_up;
public partial class PowerUp : Area2D
{
	[Export] private float _speed = 70.0f;
	[Export] private Timer _timer;
	[Export] private int _healthBoost = 30;
	[Export] private AudioStreamPlayer _boostSound;
	
	private Vector2 _velocity;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_velocity = _speed * Vector2.Down;
		Rect2 vpr = GetViewportRect();
		float xpos = (float)GD.RandRange(
			vpr.Size.X * 0.25, vpr.Size.X * 0.75
		);
		float ypos = (float)GD.RandRange(
			vpr.Size.Y * 0.15, vpr.Size.Y * 0.35
		);
		GlobalPosition = new Vector2(xpos, ypos);
		_timer.Timeout += OnTimeout;
		AreaEntered += OnAreaEntered;
		_boostSound.Finished += OnTimeout;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Position += _velocity * (float)delta;
	}

	private void OnTimeout()
	{
		CallDeferred(MethodName.QueueFree);
	}

	private void OnAreaEntered(Area2D area)
	{
		Hide();
		SpaceUtils.ActivateArea2D(this, false);
		SignalManager.EmitOnPlayerHealthBoost(_healthBoost);
		_boostSound.Play();
	}
}
