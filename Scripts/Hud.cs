using Godot;
using System;

public partial class Hud : CanvasLayer
{
    [Signal]
    public delegate void StartGameEventHandler();

    public void ShowMessage(string text)
    {
        var message = GetNode<Label>("Message");
        var messageTimer = GetNode<Timer>("MessageTimer");

        message.Text = text;
        message.Show();

        messageTimer.Start();
    }

    async public void ShowGameOver()
    {
        var messageTimer = GetNode<Timer>("MessageTimer");
        var message = GetNode<Label>("Message");
        var startButton = GetNode<Button>("StartButton");

        ShowMessage("哇，游戏结束！");

        await ToSignal(messageTimer, Timer.SignalName.Timeout);
        message.Text = "欢迎来玩";
        message.Show();

        await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
        startButton.Show();

    }

    public void UpdateScore(int score)
    {
        GetNode<Label>("ScoreLabel").Text = score.ToString();
    }

    private void OnStartButtonPressed()
    {
        GetNode<Button>("StartButton").Hide();
        EmitSignal(SignalName.StartGame);
    }

    private void OnMessageTimerTimeout()
    {
        GetNode<Label>("Message").Hide();
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
