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
        UpdatePercentageAsync();
        
    }

    public override void _Process(float delta)
    {
        GD.Print(Value);
        UpdatePercentageAsync();
    }

    public async void UpdatePercentageAsync()
    {
        await ToSignal((Timer)GetViewport().GetNode("DesktopControl/Timer"), "timeout");
        tween.SetActive(true);
        tween.InterpolateMethod(this, "set_value", Value, GetTree().GetNodesInGroup("Window").Length * 2, 1.5f, Tween.TransitionType.Linear, Tween.EaseType.In, 0);
        tween.Start();
    }
}
