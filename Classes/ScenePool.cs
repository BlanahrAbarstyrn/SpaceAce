using System.Collections.Generic;
using System.Linq;
using Godot;
using SpaceAce.Contracts;

namespace SpaceAce.Classes;
public class ScenePool
{
	private const bool DebugPool = false;

	private readonly List<Node2D> _items = new();
	private readonly PackedScene _packedScene;
	private readonly Node _container;

	private int ActiveCount => _items.Count(n => n.Visible);

	private void Log(string msg)
	{
		if (DebugPool)
		{
			GD.Print($"[ScenePool: {_packedScene.ResourcePath}] {msg}");
		}
	}

	public ScenePool(int initialCount, PackedScene scene, Node container)
	{
		_container = container;
		_packedScene = scene;
		
		Log($"Creating pool with {initialCount} items");

		for (int i = 0; i < initialCount; i++)
		{
			AddNewItem();
		}
	}

	private Node2D AddNewItem()
	{
		Node2D newItem = _packedScene.Instantiate<Node2D>();
		if (newItem is not iPool_Item)
		{
			GD.PrintErr($"{newItem} does not implement iPool_Item");
		}
		newItem.Hide();
		_container.AddChild(newItem);
		_items.Add(newItem);
		Log($"new instance {newItem.Name} total: {_items.Count}");
		return newItem;
	}

	public void ActivateNext(Vector2 position)
	{
		Log($"ActivateNext Active: {ActiveCount}/{_items.Count}");

		foreach (var node in _items)
		{
			if (!node.Visible)
			{
				Log($"Reusing instance {node.Name}");
				node.Show();
				node.GlobalPosition = position;
				((iPool_Item)node).Activate();
				return;
			}
		}
		Log($"Nothing free, extending pool");
		Node2D newItem = AddNewItem();
		newItem.Show();
		newItem.GlobalPosition = position;
		((iPool_Item)newItem).Activate();
	}
}