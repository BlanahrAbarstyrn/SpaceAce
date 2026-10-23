using Godot;
using System;
using SpaceAce.Globals;

namespace SpaceAce.Scenes.asteroid;
public partial class Asteroid : Node2D
{
	[Export] private int _hit = 2;
	[Export] private float _speed = 50.0f;
	[Export] private float _rotationSpeedDegrees = 10.0f;
	[Export] private float _lifeTime = 10.0f;

	[Export] private PackedScene _explosionScene;
	[Export] private Sprite2D _sprite;
	[Export] private Area2D _hitArea;
	[Export] private Godot.Collections.Array<Texture2D> _textures;
	
	Vector2 _velocity = Vector2.Right;
	private float _alive = 0.0f;
	
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
		//_velocity *= _speed;
		_hitArea.AreaEntered += OnHitAreaEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Position += _velocity * (float)delta;
		RotationDegrees += _rotationSpeedDegrees * (float)delta;
		_alive += (float)delta;
		if (_alive >= _lifeTime)
		{
			Blowup();
		}
	}

	private void Blowup()
	{
		SignalManager.EmitOnSpawnPoolObject(GlobalPosition, _explosionScene);
		CallDeferred(MethodName.QueueFree);
	}

	private void OnHitAreaEntered(Area2D area)
	{
		_hit--;
		if (_hit <= 0)
		{
			Blowup();
		}
	}
}
