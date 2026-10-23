using Godot;
using System;
using SpaceAce.Classes;
using SpaceAce.Globals;
using SpaceAce.Scenes.bullet;
using SpaceAce.Scenes.health_bar;

namespace SpaceAce.Scenes.enemy_base;

public partial class EnemyBase : PathFollow2D
{
	[Export] private float _speed = 50.0f;
	[Export] private Marker2D _muzzle;
	[Export] private Area2D _hitArea;
	[Export] private HealthBar _healthBar;
	[Export] private PackedScene _bulletScene;
	[Export] private PackedScene _explosionScene;
	[Export] private Timer _laserTimer;
	[Export] private float _bulletWaitTime = 3.0f;
	[Export] private float _bulletWaitTimeVariance = 0.5f;

	public override void _Ready()
	{
		_laserTimer.Timeout += OnLaserTimerTimeout;
		_healthBar.OnDied += OnHealthBarDied;
		_hitArea.AreaEntered += OnHitAreaEntered;
		SpaceUtils.SetAndStartTimer(_laserTimer, _bulletWaitTime, _bulletWaitTimeVariance);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Progress += _speed * (float)delta;
		CheckEndOfPath();
	}

	private void CheckEndOfPath()
	{
		if (ProgressRatio > 0.99)
		{
			CallDeferred(MethodName.QueueFree);
		}
	}

	private void FireWeapon()
	{
		if (_bulletScene != null)
		{
			SignalManager.EmitOnSpawnPoolObject(_muzzle.GlobalPosition, _bulletScene);
		}
	}

	private void OnLaserTimerTimeout()
	{
		FireWeapon();
		SpaceUtils.SetAndStartTimer(_laserTimer, _bulletWaitTime, _bulletWaitTimeVariance);
	}

	private void OnHealthBarDied()
	{
		SignalManager.EmitOnSpawnPoolObject(GlobalPosition, _explosionScene);
		SpaceUtils.ActivateArea2D(_hitArea, false);
		Tween tween = CreateTween();
		tween.TweenProperty(
			this, CanvasItem.PropertyName.Modulate.ToString(), new Color("ff0000"), 0.25f
			);
		tween.TweenCallback(Callable.From(QueueFree));
	}
	
	private void OnHitAreaEntered(Area2D area)
	{
		if (area is Bullet bullet && _healthBar.Value > 0)
		{
			_healthBar.TakeDamage(bullet.Damage);
		}
	}

}
