using Godot;
using System;

public class CPUTextureProgress : TextureProgress
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    private Tween tween;
    private float newValue;

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        tween = this.GetNode("Tween") as Tween;      
    }

    public override void _Process(float delta)
    {
        UpdatePercentageAsync();
        if (Value >= 100)
        {
            ((TextureRect)GetViewport().GetNode("DesktopControl/GlitchEffect")).SetVisible(true);
        } else
        {
            ((TextureRect)GetViewport().GetNode("DesktopControl/GlitchEffect")).SetVisible(false);
        }
    }

    public async void UpdatePercentageAsync()
    {
        await ToSignal((Timer)GetViewport().GetNode("DesktopControl/WindowSpawnTimer"), "timeout");
        tween.SetActive(true);
        tween.InterpolateMethod(this, "set_value", Value, Mathf.Pow(GetTree().GetNodesInGroup("Window").Length * 2,1.5f), 1.5f, Tween.TransitionType.Linear, Tween.EaseType.In, 0);
        tween.Start();
        var timer = GetViewport().GetNode("DesktopControl/WindowSpawnTimer") as WindowSpawnTimer;
        timer.DecrementWaitTime(.00005f);

    }
}
