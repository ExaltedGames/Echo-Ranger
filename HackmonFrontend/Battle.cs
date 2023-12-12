using Godot;
using Hackmon.Debugging;
using HackmonInternals.Battle;
using HackmonInternals.Models;

namespace HackmonFrontend;

public partial class Battle : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DebugPlug.Init();
	}

	// The auto-run persistent script will be in charge of passing in relevant data to this scene via this method,
	// such as the TrainerData for the player and opponent.
	public void InitBattle(TrainerData playerData, TrainerData enemy)
	{
		// init a new BattleManager object to handle fight logic
		// TODO: redo this because rework broke it.
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// InitBattle should be getting called before any processing gets done, this should make sure of that.
		// (and also keep me from having to make the entirety of event handling null-safe)
		
		//TODO: set up animations and shit
		
			//TODO: handle battle events	
	}
}
