using Godot;
using SpaceAce.Classes;
using SpaceAce.Contracts;
using SpaceAce.Globals;
using SpaceAce.Scenes.life_time;
using SpaceAce.Scenes.player;

namespace SpaceAce.Scenes.bullet;
public partial class Bullet : Area2D, iPool_Item
{
	[Export] private float _bulletSpeed = 350;
	[Export] private Vector2 _bulletDirection = Vector2.Zero;
	[Export] private bool _towardsPlayer = false;
	[Export] private VisibleOnScreenNotifier2D _notifier;
	[Export] private LifeTime _lifeTime;
	[Export] private PackedScene _explosionScene;
	[Export] private Marker2D _explosionMarker;
	[Export] private bool _usesMarker;
	[Export] private float _explosionMargin = 30.0f;
	[Export] private int _damage = 5;
	
	public int Damage => _damage;
	
	private Vector2 _velocity;
	
	// keep variable as Player type ie "Player _player"
	private Player _player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (_towardsPlayer)
		{	
			// GroupName was declared in Player
			// need to cast "as Player" because "GetFirstNodeInGroup" is type Node
			_player = GetTree().GetFirstNodeInGroup(Player.GroupName) as Player;
			if (_player == null)
			{
				GD.PrintErr("No player found");
			}
		}
		
		_lifeTime.OnLifeTimeExpired += OnLifeTimeExpired;
		_notifier.ScreenExited += OnScreenExited;
		AreaEntered += OnAreaEntered;
		Deactivate(); // start hidden and ready to use
		//Activate(); // temporary testing
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Position += _velocity * (float)delta;
	}

	private void OnLifeTimeExpired()
	{
		//GD.Print($"{Name} OnLifeTimeExpired");
		//CallDeferred(MethodName.QueueFree);
	}

	private void OnScreenExited()
	{
		GD.Print($"{Name} OnScreenExited");
		Deactivate();
	}

	private void OnAreaEntered(Area2D area)
	{
		GD.Print($"{Name} OnAreaEntered");

		var explosionPosition = _explosionMarker.GlobalPosition;
		
		if (!_usesMarker)
		{
			Vector2 explosionDirection = GlobalPosition.DirectionTo(area.GlobalPosition);
			explosionPosition = GlobalPosition + explosionDirection * _explosionMargin;
		}
		
		SignalManager.EmitOnSpawnPoolObject(
			explosionPosition,
			_explosionScene
		);
		
		Deactivate();
	}

	public void Activate()
	{
		SpaceUtils.ActivateArea2D(this, true);
		if (_towardsPlayer && _player != null)
		{
			// need to start at sprite's location but travel to player's location
			_bulletDirection = GlobalPosition.DirectionTo(_player.GlobalPosition);
		}
		_velocity = _bulletDirection * _bulletSpeed;
		SetPhysicsProcess(true);
	}

	public void Deactivate()
	{
		SpaceUtils.ActivateArea2D(this, false);
		SetPhysicsProcess(false);
		Hide();
	}
}
