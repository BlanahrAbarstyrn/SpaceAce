using Godot;
using System;

namespace SpaceAce.Classes;

[GlobalClass]
public partial class Wave : Resource
{
    [Export] public PackedScene EnemyScene { get; set; }
    [Export] public int EnemyCount  { get; set; }
    [Export] public float SpawnInterval { get; set; }
    [Export] public float WaveInterval { get; set; }
    

    public Wave()
    {
        
    }
}
