using Godot;
using System;
using SpaceAce.Classes;
using SpaceAce.Globals;
using SpaceAce.Scenes.player;

namespace SpaceAce.Scenes.asteroid;
public partial class Asteroid : Node2D
{
	[Export] private int _hit = 2;
	[Export] private float _speed = 50.0f;
	[Export] private float _rotationSpeedDegrees = 10.0f;
	[Export] private Timer _lifeTimer;

	[Export] private PackedScene _explosionScene;
	[Export] private Sprite2D _sprite;
	[Export] private Area2D _hitArea;
	[Export] private Godot.Collections.Array<Texture2D> _textures;
	[Export] private int _points = 2;
	
	Vector2 _velocity = Vector2.Right;
	
	private Player _player;
	private bool _isVibrating = SpaceUtils.IsJoyVibrating(0);
	
	public void SetSpawnAndTarget(Vector2 spawnPosition, Vector2 targetPosition)
	{
		GlobalPosition = spawnPosition;
		_velocity = spawnPosition.DirectionTo(targetPosition) * _speed * (float)GD.RandRange(0.5, 1.5);
		_rotationSpeedDegrees *= (float)GD.RandRange(0.5, 1.5);
		_rotationSpeedDegrees *= GD.Randf() > 0.5f ? 1.0f : -1.0f;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sprite.Texture = _textures.PickRandom();
		_hitArea.AreaEntered += OnHitAreaEntered;
		_lifeTimer.Timeout += Blowup;
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Position += _velocity * (float)delta;
		RotationDegrees += _rotationSpeedDegrees * (float)delta;
		
	}

	private void Blowup()
	{
		SignalManager.EmitOnSpawnPoolObject(GlobalPosition, _explosionScene);
		CallDeferred(MethodName.QueueFree);
	}

	private void OnHitAreaEntered(Area2D area)
	{
		if (area is Player)
		{
			if (_isVibrating)
			{
				Input.StopJoyVibration(0);
				Input.StartJoyVibration(0, 1.0f, 1.0f, 1.0f);
			}
			else
			{
				Input.StartJoyVibration(0, 1.0f, 1.0f, 1.0f);
			}
			Blowup();
		}
		else
		{
			_hit--;
			if (_hit <= 0)
			{
				Blowup();
				SignalManager.EmitOnScorePoints(_points);
			}
		}
	}
}
