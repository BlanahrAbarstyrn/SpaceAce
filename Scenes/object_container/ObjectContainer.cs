using Godot;
using SpaceAce.Classes;
using SpaceAce.Globals;
using System.Collections.Generic;

namespace SpaceAce.Scenes.object_container;
public partial class ObjectContainer : Node
{
	[Export] private PackedScene _testScene;
	
	private readonly Dictionary<PackedScene, ScenePool> _pools = new();

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("test"))
		{
			SignalManager.EmitOnSpawnPoolObject(
				new Vector2(
					GD.Randf() * 800,
					GD.Randf() * 300
					),
				_testScene
				);
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SignalManager.Instance.OnSpawnPoolObject += OnSpawnPoolObject;
	}

	public override void _ExitTree()
	{
		SignalManager.Instance.OnSpawnPoolObject -= OnSpawnPoolObject;
	}
	
	private void OnSpawnPoolObject(Vector2 pos, PackedScene scene)
	{
		CallDeferred(MethodName.SpawnDeferred, pos, scene);
	}

	private void SpawnDeferred(Vector2 pos, PackedScene scene)
	{
		if (!_pools.TryGetValue(scene, out var pool))
		{
			pool = new ScenePool(5, scene, this);
			_pools.Add(scene, pool);
		}
		
		pool.ActivateNext(pos);
	}
}
