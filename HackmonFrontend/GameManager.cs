using System;
using System.Collections.Generic;
using Godot;
using Hackmon.Debugging;
using HackmonInternals;
using HackmonInternals.Models;

namespace HackmonFrontend;

public partial class GameManager : Node
{
	public Node CurrentScene { get; set; }
	public TrainerData PlayerData { get; set; }
	private TrainerData CurrentOpponent { get; set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		DebugPlug.Init();
		var root = GetTree().Root;
		CurrentScene = root.GetChild(root.GetChildCount() - 1);
		
		HackmonManager.LoadAllData();
		GD.Print("Data loaded.");

		PlayerData = new();
		var testOpponent = new TrainerData();
		var playerMon = new HackmonInstance(HackmonManager.HackmonRegistry[0], 90);
		var enemyMon = new HackmonInstance(HackmonManager.HackmonRegistry[0], 20);
		var playerTeam = new List<HackmonInstance>() { playerMon }; 
		var enemyTeam = new List<HackmonInstance>() { enemyMon };

		PlayerData.CurrentParty = playerTeam;
		testOpponent.CurrentParty = enemyTeam;
		
		GD.Print("Now entering: test battle.");
		EnterBattle(testOpponent);
	}

	public void Save()
	{
		var saveFile = FileAccess.Open("user://PlayerData.json", FileAccess.ModeFlags.Write);
		
	}

	public void EnterBattle(TrainerData opponent)
	{
		CurrentOpponent = opponent;	
		CallDeferred(nameof(DeferredEnterBattle));
	}

	public void ChangeScene(string scenePath)
	{
		CallDeferred(nameof(DeferredChangeScene), scenePath);
	}

	private void DeferredEnterBattle()
	{
		CurrentScene.QueueFree();
		var battle = GD.Load<PackedScene>("res://Battle.tscn");
		CurrentScene = battle.Instantiate();
		GetTree().Root.AddChild(CurrentScene);
		GetTree().CurrentScene = CurrentScene;

		var battleScene = (Battle)CurrentScene;
		battleScene.InitBattle(PlayerData, CurrentOpponent);
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
