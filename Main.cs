using Godot;
using System;

public class Main : Spatial
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
    private Control desktop;
    public override void _Ready()
    {
        OS.SetWindowMaximized(true);
        Input.SetMouseMode(Input.MouseMode.Captured);
        viewport = GetNode("Viewport") as Viewport;
        cursor = GetNode("Viewport/DesktopControl/CursorControl") as CursorControl;
        GD.Print("Main script on");
        audioClick = GetNode("/root/Main/MonitorSpatial/Click") as AudioStreamPlayer3D;
        audioUnclick = GetNode("/root/Main/MonitorSpatial/Unclick") as AudioStreamPlayer3D;
        audioKeyboardClick = GetNode("/root/Main/MonitorSpatial/KeyboardClick") as AudioStreamPlayer3D;
        desktop = GetNode("Viewport/DesktopControl") as Control;
    }

    public override void _Process(float delta)
    {
        var windowLayer = desktop.GetNode("WindowLayer") as Control;
        if (Input.IsActionJustPressed("ui_select"))
            audioClick.Play(0);
        if (Input.IsActionJustReleased("ui_select"))
            audioUnclick.Play(0);
        if (Input.IsActionJustPressed("ui_cancel"))
            Toggle_escape();
        if (Input.IsActionJustPressed("ui_new"))
            desktop.AddChildBelowNode(windowLayer, ((PackedScene)ResourceLoader.Load("res://WindowBasicContainer.tscn")).Instance());
        if (Input.IsActionJustPressed("ui_select"))
        {
            GD.Print("viewport fake click ", pos);
            viewport.Input(FakeClick(pos, true));
        }
        else
        {
            viewport.Input(FakeClick(pos, false));
        }
    }

    public override void _Input(InputEvent @event)
    {


        if (@event is InputEventMouse inputEventMouse)
        {
            pos.x = -1 + (int)cursor.pos.x;
            pos.y = -1 + (int)cursor.pos.y;
            inputEventMouse.Position = pos;
            inputEventMouse.GlobalPosition = pos;

            viewport.Input(@event);
        }
        if (@event is InputEventMouseMotion eventMouseMotion && !escaped)
        {
            cursor.pos += eventMouseMotion.Relative;
        }
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.IsPressed())
                audioKeyboardClick.Play(0);
        }


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

    public InputEventMouseButton FakeClick(Vector2 position, bool isPressed)
    {
        var @event = new InputEventMouseButton()
        {
            ButtonIndex = (int)ButtonList.Left,
            Pressed = isPressed,
            Position = position,
            GlobalPosition = position
        };
        return @event;
    }

    public void MoveCursor(InputEventMouse inputEventMouse)
    {
        pos.x = -1 + (int)cursor.pos.x;
        pos.y = -1 + (int)cursor.pos.y;
        inputEventMouse.Position = pos;
        inputEventMouse.GlobalPosition = pos;
        var @event = inputEventMouse;
        viewport.Input(@event);
    }

}
