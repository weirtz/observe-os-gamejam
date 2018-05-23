using Godot;
using System;


public class Main : Spatial
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    public Vector2 cursorPos;
    public Vector2 pos;
    private Viewport viewport;
    public bool escaped;
    private AudioStreamPlayer3D audioClick;
    private AudioStreamPlayer3D audioUnclick;
    private AudioStreamPlayer3D audioKeyboardClick;
    private Node desktop;
    private SpotLight spotLight;

    public override void _Ready()
    {
        OS.SetWindowMaximized(true);
        Input.SetMouseMode(Input.MouseMode.Captured);
        viewport = GetNode("Viewport") as Viewport;
        spotLight = GetNode("SpotLight") as SpotLight;
        GD.Print("Main script on");
        audioClick = GetNode("/root/Main/MonitorSpatial/Click") as AudioStreamPlayer3D;
        audioUnclick = GetNode("/root/Main/MonitorSpatial/Unclick") as AudioStreamPlayer3D;
        audioKeyboardClick = GetNode("/root/Main/MonitorSpatial/KeyboardClick") as AudioStreamPlayer3D;
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_cancel"))
            Toggle_escape();
    }

    public void Toggle_escape()
    {
        if (escaped == true)
        {
            Input.SetMouseMode(Input.MouseMode.Captured);
            escaped = false;
        }
        else if (escaped == false)
        {
            Input.SetMouseMode(Input.MouseMode.Visible);
            escaped = true;
        }
        else
        {
            Input.SetMouseMode(Input.MouseMode.Captured);
            escaped = false;
        }

    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion eventMouseMotion && !escaped)
        {
            cursorPos += eventMouseMotion.Relative;
            pos.x = -1 + (int)cursorPos.x;
            pos.y = -1 + (int)cursorPos.y;
            eventMouseMotion.Position = pos;
            eventMouseMotion.GlobalPosition = pos;
            viewport.Input(@event);
        }
        if (@event is InputEventMouseButton eventMouseButton)
        {
            eventMouseButton.Position = pos;
            eventMouseButton.GlobalPosition = pos;
            viewport.Input(@event);
        }
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.IsPressed())
                audioKeyboardClick.Play(0);
        }
    }
    public void OnBootTimerTimeout()
    {
        //smoothly applies light to the scene
        var tween = (Tween)spotLight.GetNode("LightTween");
        tween.SetActive(true);
        tween.InterpolateProperty(spotLight, "light_energy", 0, 16, 1.5f, Tween.TransitionType.Linear, Tween.EaseType.In);
        tween.Start();
        //Instances a new desktop instance
        var newDesktop = ((PackedScene)ResourceLoader.Load("res://DesktopControl.tscn")).Instance() as DesktopControl;
        newDesktop.SetName("DesktopControl");
        viewport.CallDeferred("add_child", newDesktop, false);
        //removes the boot video from sight
        ((VideoPlayer)viewport.GetNode("VideoPlayer")).SetVisible(false);
    }
}
