using Godot;
using System;

public class WindowOpener : Node
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    private Node desktop;


    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        desktop = GetViewport().GetNode("DesktopControl") as Node;
        desktop.GetNode("WindowSpawnTimer").Connect("timeout", this, "Spawn");
        Spawn();
    }

    //    public override void _Process(float delta)
    //    {
    //        // Called every frame. Delta is time since last frame.
    //        // Update game logic here.
    //        
    //    }

    public void Spawn()
    {
        var windowLayer = desktop.GetNode("WindowLayer") as Control;
        var newWindow = ((PackedScene)ResourceLoader.Load("res://WindowBasicContainer.tscn")).Instance() as WindowBasicContainer;
        var rand = new Random();
        desktop.CallDeferred("add_child_below_node", windowLayer, newWindow, true);

        var size = new Vector2
        {
            x = rand.Next(1500, 5000),
            y = rand.Next(1500, 3500)
        };

        var pos = new Vector2
        {
            x = rand.Next(0, (int)GetViewport().Size.x - (int)(size.x* newWindow.RectScale.x)),
            y = rand.Next(0, (int)GetViewport().Size.y - (int)(size.y* newWindow.RectScale.y))
        };
        newWindow.SetPosition(pos - newWindow.GetRealRectSize());
        newWindow.SetSize(size);


    }
}
