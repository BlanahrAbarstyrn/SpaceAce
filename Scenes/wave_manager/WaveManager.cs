using Godot;
using System;
using SpaceAce.Classes;
using SpaceAce.Scenes.enemy_base;

namespace SpaceAce.Scenes.wave_manager;
public partial class WaveManager : Node
{
	[Export] private Godot.Collections.Array<Wave> _waves = new();
	[Export] private Timer _waveTimer;
	[Export] private Timer _shipTimer;
	[Export] private Node _pathsHolder;
	
	private Wave _currentWave;
	private Path2D _currentPath;
	private Godot.Collections.Array<Path2D> _pathsList = new();
	private int _currentWaveIndex = -1;
	private int _enemiesSpawned = 0; 
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (var node in _pathsHolder.GetChildren())
		{
			if(node is Path2D path) 
			{
				_pathsList.Add(node as Path2D);
			}
		}

		if (_pathsList.Count == 0 || _waves.Count == 0)
		{
			GD.PrintErr("No waves or paths found");
			return;
		}

		_waveTimer.Timeout += StartNextWave;
		_shipTimer.Timeout += OnShipTimerTimeout;
		
		StartNextWave();
	}

	private void StartNextWave()
	{
		_currentWaveIndex = (_currentWaveIndex + 1) % _waves.Count;
		_currentWave = _waves[_currentWaveIndex];
		_currentPath = _pathsList.PickRandom();
		_enemiesSpawned = 0;
		_shipTimer.Start(_currentWave.SpawnInterval);
	}

	private void OnShipTimerTimeout()
	{
		var enemy = _currentWave.EnemyScene.Instantiate<EnemyBase>();
		_currentPath.AddChild(enemy);
		_enemiesSpawned++;

		if (_enemiesSpawned == _waves.Count)
		{
			_shipTimer.Stop();
			_waveTimer.Start(_currentWave.WaveInterval);
		}
		else
		{
			_shipTimer.Start();
		}
	}
}
