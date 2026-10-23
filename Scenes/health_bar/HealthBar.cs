using Godot;
using System;

namespace SpaceAce.Scenes.health_bar;
public partial class HealthBar : TextureProgressBar
{
	[Signal]
	public delegate void OnDiedEventHandler();
	
	private static readonly Color ColorDanger = new ("9A33FF");
	private static readonly Color ColorMiddleHealth = new ("FF00CC");
	private static readonly Color ColorHighHealth = new ("00FFC8");

	[Export] private int _levelLow = 30;
	[Export] private int _levelMedium = 60;
	[Export] private int _maxHealth = 100;
	[Export] private int _startHealth = 100;

	private bool _isDead;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_isDead = false;
		MaxValue = _maxHealth;
		Value = _startHealth;
		SetColor();
	}

	private void SetColor()
	{
		if (Value < _levelLow)
		{
			TintProgress = ColorDanger;
		}
		else if (Value < _levelMedium)
		{
			TintProgress = ColorMiddleHealth;
		}
		else
		{
			TintProgress = ColorHighHealth;
		}
	}

	public void IncrementValue(int value)
	{
		Value += value;
		if (Value <= 0 && !_isDead)
		{
			_isDead = true;
			EmitSignal(SignalName.OnDied);
		}
		SetColor();
	}

	public void TakeDamage(int value)
	{
		IncrementValue(-value);
	}
}
