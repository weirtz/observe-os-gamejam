using Godot;
using System;



public class WindowBasicContainer : MarginContainer
{
    public Vector2 pos;
    public bool isDragging;
    private CursorControl cursor;
    private Control grabber;
    private Vector2 offset;
    private Rect2 grabberRect;

    public override void _Ready()
    {
        pos = GetPosition();
        isDragging = false;
        cursor = GetParent().GetNode("CursorControl") as CursorControl;
        grabber = GetNode("VBoxContainer/MarginContainer/GrabberContainer/GrabberControl") as Control;
    }

    public override void _Process(float delta)
    {
        if (isDragging)
            SetPosition(cursor.pos + offset);
    }

    public override void _GuiInput(InputEvent @event)
    {
        grabberRect = new Rect2(RectPosition.x, RectPosition.y, grabber.GetRect().Size.x * RectScale.x, grabber.GetRect().Size.y * RectScale.y);

        if (grabberRect.HasPoint(cursor.pos) && Input.IsActionPressed("ui_select"))
        {
            offset = RectPosition - cursor.pos;
            isDragging = true;
        } else if (Input.IsActionJustReleased("ui_select"))
        {
            grabberRect = new Rect2(RectPosition.x, RectPosition.y, grabber.GetRect().Size.x * RectScale.x, grabber.GetRect().Size.y * RectScale.y);
            isDragging = false;
        }
    }
    public Vector2 GetRealRectSize()
    {
        return RectSize * RectScale;
    }

    public Vector2 GetRealMinRectSize()
    {
        return RectMinSize * RectScale;
    }
    public void OnCloseButtonPressed()
    {
        QueueFree();
    }
    public void OnMaxButtonPressed()
    {
        this.RectPosition = new Vector2(0, 0);
        this.RectSize = GetViewport().Size / RectScale;
    }
}
