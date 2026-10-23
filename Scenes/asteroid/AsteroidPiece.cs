using Godot;
using System;

namespace SpaceAce.Scenes.asteroid;
public partial class AsteroidPiece : Node2D
{
	[Export] private float _speed = 350.0f;
	[Export] private float _rotationSpeedDegreesBase = 400.0f;
	[Export] private Godot.Collections.Array<Texture2D> _textures;
	[Export] private Sprite2D _sprite;
	
	Vector2 _velocity = Vector2.Right;
	float _rotationSpeedDegrees = 0.0f;
	Transform2D _startTransform;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_startTransform = Transform;
		Reset();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += _velocity * (float)delta;
		RotationDegrees += _rotationSpeedDegrees * (float)delta;
	}

	public void Reset()
	{
		Transform = _startTransform;
		_rotationSpeedDegrees = _rotationSpeedDegreesBase;
		_rotationSpeedDegrees *= (float)GD.RandRange(0.6, 1.2);
		_rotationSpeedDegrees *= GD.Randf() > 0.5f ? 1.0f : -1.0f;
		_velocity = Position.Normalized() * _speed * (float)GD.RandRange(0.8,1.2);
		_sprite.Texture = _textures.PickRandom();
		SetProcess(false);
	}
}
