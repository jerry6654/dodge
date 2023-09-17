using Godot;
using System;


public partial class Main : Node2D
{
    [Export]
    public PackedScene MobScene { get; set; }

    private int _score;

    public void GameOver()
    {
        var music = GetNode<AudioStreamPlayer2D>("Music");
        var deathSound = GetNode<AudioStreamPlayer2D>("DeathSound");
        var mobTimer = GetNode<Timer>("MobTimer");
        var scoreTimer = GetNode<Timer>("ScoreTimer");
        var hub = GetNode<Hud>("HUD");

        mobTimer.Stop();
        scoreTimer.Stop();
        hub.ShowGameOver();
        music.Stop();
        deathSound.Play();

    }

    public void NewGame()
    {
        var deathSound = GetNode<AudioStreamPlayer2D>("Music");
        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Marker2D>("StartPosition");
        var startTimer = GetNode<Timer>("StartTimer");
        var hud = GetNode<Hud>("HUD");

        _score = 0;

        player.Start(startPosition.Position);
        startTimer.Start();

        hud.UpdateScore(_score);
        hud.ShowMessage("Get Ready!");

        // 貌似无效
        GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
        deathSound.Play();

    }

    private void OnScoreTimerTimeout()
    {
        _score++;
        GetNode<Hud>("HUD").UpdateScore(_score);
    }

    private void OnStartTimerTimeout()
    {
        var mobTimer = GetNode<Timer>("MobTimer");
        var scoreTimer = GetNode<Timer>("ScoreTimer");

        mobTimer.Start();
        scoreTimer.Start();
    }

    private void OnMobTimerTimeout()
    {
        Mob mob = MobScene.Instantiate<Mob>();
        var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");

        mobSpawnLocation.ProgressRatio = GD.Randf();

        float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;
        mob.Position = mobSpawnLocation.Position;

        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
        mob.Rotation = direction;

        var velocity = new Vector2((float)GD.RandRange(150, 250), 0);
        mob.LinearVelocity = velocity.Rotated(direction);

        AddChild(mob);
    }

    public override void _Ready()
    {
        // NewGame();
    }
}
