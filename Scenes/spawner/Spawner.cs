using Godot;
using System;
using SpaceAce.Scenes.asteroid;
using SpaceAce.Scenes.power_up;

namespace SpaceAce.Scenes.spawner;
public partial class Spawner : Node
{
	[Export] private Path2D _asteroidSpawnPath;
	[Export] private Path2D _targetSpawnPath;
	[Export] private PackedScene _asteroidScene;
	[Export] private Timer _asteroidSpawnTimer;
	[Export] private PackedScene _powerUpScene;
	[Export] private Timer _powerUpSpawnTimer;

	private Vector2 GetRandomPointOnCurve(Curve2D curve)
	{
		float distance = curve.GetBakedLength() * GD.Randf();
		return curve.SampleBaked(distance);
	}

	private void SetRandomSpawnAndTarget(Asteroid asteroid)
	{
		var spawnPosition = GetRandomPointOnCurve(_asteroidSpawnPath.Curve);
		var targetPosition = GetRandomPointOnCurve(_targetSpawnPath.Curve);
		
		asteroid.SetSpawnAndTarget(spawnPosition, targetPosition);
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_asteroidSpawnTimer.Timeout += OnAsteroidSpawnTimerTimeout;
		_powerUpSpawnTimer.Timeout += OnPowerUpSpawnTimerTimeout;
		OnAsteroidSpawnTimerTimeout();
	}
	
	private void OnAsteroidSpawnTimerTimeout()
	{
		Asteroid asteroid = _asteroidScene.Instantiate<Asteroid>();
		SetRandomSpawnAndTarget(asteroid);
		AddChild(asteroid);
	}

	private void OnPowerUpSpawnTimerTimeout()
	{
		PowerUp powerUp = _powerUpScene.Instantiate<PowerUp>();
		AddChild(powerUp);
	}
}
