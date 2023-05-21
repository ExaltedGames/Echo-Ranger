using Godot;
using System;
using HackmonInternals;

public partial class GameManager : Node
{
	public Node CurrentScene { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var root = GetTree().Root;
		CurrentScene = root.GetChild(root.GetChildCount() - 1);
	}

	public void EnterBattle(HackmonData user, HackmonData opponent)
	{
		
	}

	public void ChangeScene(string scenePath)
	{
		CallDeferred(nameof(DeferredChangeScene), scenePath);
	}

	private void DeferredEnterBattle(HackmonData user, HackmonData opponent)
	{
		CurrentScene.QueueFree();
		var battle = GD.Load<Battle>("res://Battle/Battle.tscn");
		
	}
	
	private void DeferredChangeScene(string path)
	{
		CurrentScene.QueueFree();
		var nextScene = GD.Load<PackedScene>(path);
		CurrentScene = nextScene.Instantiate();
		GetTree().Root.AddChild(CurrentScene);
		GetTree().CurrentScene = CurrentScene;
	}
}
