using Godot;
using System;
using System.Collections.Generic;
using SpaceAce.Contracts;

namespace SpaceAce.Scenes.asteroid;
public partial class AsteroidExplosion : Node2D, iPool_Item
{
	[Export] private Timer _timer;
	[Export] private Node2D _piecesHolder;
	[Export] private AudioStreamPlayer _asteroidExplosionSound;

	private List<AsteroidPiece> _pieces = new();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (var node in _piecesHolder.GetChildren())
		{
			if (node is AsteroidPiece piece)
			{
				_pieces.Add(piece);
			}
		}

		_timer.Timeout += Deactivate;
	}

	public void Activate()
	{
		Show();
		foreach (var piece in _pieces)
		{
			piece.SetProcess(true);
		}
		_timer.Start();
		_asteroidExplosionSound.Play();
	}

	public void Deactivate()
	{
		Hide();
		foreach (var piece in _pieces)
		{
			piece.Reset();
		}
	}
}
