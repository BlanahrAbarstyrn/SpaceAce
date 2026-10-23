using Godot;
using System;
using SpaceAce.Contracts;

namespace SpaceAce.Scenes.explosion;
public partial class Explosion : Node2D, iPool_Item
{
	[Export] private CpuParticles2D _cpuParticles;
	[Export] private AudioStreamPlayer2D _audioStreamPlayer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_audioStreamPlayer.Finished += Deactivate;
	}
	
	
	public void Activate()
	{
		_cpuParticles.Restart();
		_audioStreamPlayer.Play();
	}

	public void Deactivate()
	{
		Hide();
	}
}
