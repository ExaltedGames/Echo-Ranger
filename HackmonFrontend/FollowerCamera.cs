using Godot;
using System;

public partial class FollowerCamera : Camera2D
{
    [Export]
    public NodePath FollowObjectPath;

    public Node2D FollowObject;

    public override void _Ready()
    {
        FollowObject = GetNode<Node2D>(FollowObjectPath);
    }

    public override void _Process(double delta)
    {
        Position = FollowObject.Position;
    }
}
