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
	
	[Signal]
	public delegate void OnPlayerTakeDamageEventHandler(int damage);

	public static void EmitOnPlayerTakeDamage(int damage)
	{
		Instance.EmitSignal(SignalName.OnPlayerTakeDamage, damage);
	}
	
	[Signal]
	public delegate void OnPlayerHealthBoostEventHandler(int boost);

	public static void EmitOnPlayerHealthBoost(int boost)
	{
		Instance.EmitSignal(SignalName.OnPlayerHealthBoost, boost);
	}
	
	[Signal]
	public delegate void OnScorePointsEventHandler(int points);

	public static void EmitOnScorePoints(int points)
	{
		Instance.EmitSignal(SignalName.OnScorePoints,  points);
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}
	
}
