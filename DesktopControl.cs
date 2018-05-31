using Godot;
using System;

public class DesktopControl : Node
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    public CursorControl cursor;
    public Vector2 pos;
    private Viewport viewport;
    public bool escaped;
    private AudioStreamPlayer3D audioClick;
    private AudioStreamPlayer3D audioUnclick;
    private AudioStreamPlayer3D audioKeyboardClick;
    private Main main;
    public override void _Ready()
    {
        viewport = GetViewport();
        audioClick = GetNode("/root/Main/MonitorSpatial/Click") as AudioStreamPlayer3D;
        audioUnclick = GetNode("/root/Main/MonitorSpatial/Unclick") as AudioStreamPlayer3D;
        audioKeyboardClick = GetNode("/root/Main/MonitorSpatial/KeyboardClick") as AudioStreamPlayer3D;
        cursor = GetNode("CursorControl") as CursorControl;
        escaped = ((Main)GetNode("/root/Main")).escaped;
        main = ((Main)GetNode("/root/Main"));
        GD.Print("New Desktop");
    }

    public override void _Process(float delta)
    {
        var windowLayer = GetNode("WindowLayer") as Control;
        if (Input.IsActionJustPressed("ui_select"))
            audioClick.Play(0);
        if (Input.IsActionJustReleased("ui_select"))
            audioUnclick.Play(0);
        if (Input.IsActionJustPressed("ui_new"))
            AddChildBelowNode(windowLayer, ((PackedScene)ResourceLoader.Load("res://WindowBasicContainer.tscn")).Instance());
        cursor.pos = main.cursorPos;
    }
    
}
