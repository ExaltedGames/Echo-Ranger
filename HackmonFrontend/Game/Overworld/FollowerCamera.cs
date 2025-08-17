namespace HackmonFrontend.Game.Overworld;

public partial class FollowerCamera : Camera2D
{
	[Export]
	public NodePath FollowObject;

	private Node2D _followObject;

	public override void _Ready()
	{
		_followObject = GetNode<Node2D>(FollowObject);
	}

	public override void _Process(double delta)
	{
		Position = _followObject.Position;
	}
}