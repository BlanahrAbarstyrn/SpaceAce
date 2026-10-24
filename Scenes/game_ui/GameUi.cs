using Godot;
using SpaceAce.Globals;
using SpaceAce.Scenes.health_bar;
using SpaceAce.Scenes.player;

namespace SpaceAce.Scenes.game_ui;
public partial class GameUi : Control
{
	[Export] private HealthBar _healthBarScene;
	[Export] private Label _scoreLabel;
	[Export] private ColorRect _gameOverColorRect;
	[Export] AudioStreamPlayer _gameMusic;
	[Export] private AudioStreamPlayer _gameOverSound;

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			GetTree().ReloadCurrentScene();
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetTree().Paused = false;
		_gameOverColorRect.Hide();
		_healthBarScene.OnDied += OnDied;
		SignalManager.Instance.OnPlayerTakeDamage += OnPlayerTakeDamage;
	}
	
	private void OnDied()
	{
		_gameMusic.Stop();
		_gameOverSound.Play();
		_gameOverColorRect.Show();
		GetTree().Paused = true;
	}

	private void OnPlayerTakeDamage(int damage)
	{
		_healthBarScene.TakeDamage(damage);
	}
}
