using Godot;
using System;

namespace SpaceAce.Globals;
public partial class SignalManager : Node
{
	public static SignalManager Instance { get; private set; }
	
	[Signal]
	public delegate void OnSpawnPoolObjectEventHandler(Vector2 pos, PackedScene scene);

	public static void EmitOnSpawnPoolObject(Vector2 pos, PackedScene scene)
	{
		Instance.EmitSignal(SignalName.OnSpawnPoolObject, pos, scene);
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}
	
}
