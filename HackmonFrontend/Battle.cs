using Godot;
using Hackmon.Debugging;

namespace HackmonFrontend;

public partial class Battle : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DebugPlug.Init();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
