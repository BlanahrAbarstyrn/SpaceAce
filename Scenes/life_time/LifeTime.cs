using Godot;
using System;

namespace SpaceAce.Scenes.life_time;
public partial class LifeTime : Node
{
	[Export] private float _lifeTime = 2.0f;
	[Export] private Timer _lifeTimeTimer;
	
	[Signal] public delegate void OnLifeTimeExpiredEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_lifeTimeTimer.Start();
		_lifeTimeTimer.Timeout += OnTimeOut;
	}

	private void OnTimeOut()
	{
		EmitSignal(SignalName.OnLifeTimeExpired);
	}
}
