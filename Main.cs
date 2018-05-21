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

    public override void _Ready()
    {
        Input.SetMouseMode(Input.MouseMode.Captured);
        viewport = GetNode("Viewport") as Viewport;
        cursor = GetNode("Viewport/DesktopControl/CursorControl") as CursorControl;
        GD.Print("Main script on");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_cancel"))
            Toggle_escape();
    }

    public override void _Input(InputEvent @event)
    {
        // Mouse in viewport coordinates
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
}
