using Godot;
using System;

public class CursorControl : Control
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    public Vector2 pos;
    private Vector2 rectScale;

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        rectScale = RectScale;
    }

    //    public override void _Process(float delta)
    //    {
    //        // Called every frame. Delta is time since last frame.
    //        // Update game logic here.
    //        
    //    }
    public override void _Process(float delta)
    {
        SetPosition(pos);
        if (Input.IsActionJustPressed("ui_select"))
            RectScale = RectScale / (new Vector2((float)1.1, (float)1.1));
        if (Input.IsActionJustReleased("ui_select"))
            RectScale = this.rectScale;

    }
}
