using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;
using Hackmon.Debugging;
using HackmonInternals;
using HackmonInternals.Enums;
using HackmonInternals.Models;

namespace HackmonFrontend;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    
    public Node CurrentScene { get; set; }
    public Node BackgroundScene { get; set; }
    public TrainerData PlayerData { get; set; }
    private TrainerData CurrentOpponent { get; set; }
    
    /// <summary>
    /// Use this event to reactivate certain must-haves, e.g. CurrentCamera etc
    /// </summary>
    public event Action OnSceneReturned;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Instance = this;
        DebugPlug.Init();
        var root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
        if (CurrentScene is Battle)
        {
            LoadTestBattleData();
        }
    }

    public void LoadTestBattleData()
    {
        var root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
        HackmonManager.LoadAllData();
        GD.Print("Data loaded.");

        PlayerData = new();
        var testOpponent = new TrainerData();
        var playerMon = new HackmonInstance(HackmonManager.HackmonRegistry[1], 1);
        var enemyMon = new HackmonInstance(HackmonManager.HackmonRegistry[1], 1);
        var playerTeam = new List<HackmonInstance>() { playerMon };
        var enemyTeam = new List<HackmonInstance>() { enemyMon };

        PlayerData.CurrentParty = playerTeam;
        testOpponent.CurrentParty = enemyTeam;

        GD.Print("Now entering: test battle.");
        EnterBattle(testOpponent);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("Save"))
        {
            Save();
        }
        else if (@event.IsActionPressed("Load"))
        {
            GD.Print("Test one");
            Load();
            GD.Print("Load success?");
            var testOpponent = new TrainerData();
            var enemyMon = new HackmonInstance(HackmonManager.HackmonRegistry[2], 1);
            testOpponent.CurrentParty = new List<HackmonInstance>() { enemyMon };
            EnterBattle(testOpponent);
        }
    }

    public void Save()
    {
        using var saveFile = FileAccess.Open("user://PlayerData.json", FileAccess.ModeFlags.Write);
        var jsonOpts = new JsonSerializerOptions()
        {
            Converters = { new JsonStringEnumConverter() },
            WriteIndented = true,
            IgnoreReadOnlyProperties = true
        };
        var jsonText = JsonSerializer.Serialize(PlayerData, jsonOpts);
        saveFile.StoreString(jsonText);
        GD.Print("successfully saved the game.");
    }

    public void Load()
    {
        using var saveFile = FileAccess.Open("user://PlayerData.json", FileAccess.ModeFlags.Read);
        var saveData = JsonSerializer.Deserialize<TrainerData>(saveFile.GetAsText());

        PlayerData = saveData ?? throw new Exception("Failed to properly load save data. Error when parsing file.");
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

    public void DeferredPopCurrentScene()
    {
        CallDeferred(nameof(PopCurrentScene));
    }

    public static void SaveData<T>(T dataObj)
    {
        var savePath = dataObj.GetType().Name;
        using var saveFile = FileAccess.Open($"user://{savePath}.json", FileAccess.ModeFlags.Write);
        var jsonOpts = new JsonSerializerOptions()
        {
            Converters = { new JsonStringEnumConverter() },
            WriteIndented = true,
            IgnoreReadOnlyProperties = true
        };
        var jsonText = JsonSerializer.Serialize(dataObj, typeof(T), jsonOpts);
        saveFile.StoreString(jsonText);
    }
    
    // TODO Both of these "DeferredX" functions should not be called such as they are not deferred.
    // Call the functions that call them deferred Deferred.
    private void DeferredEnterBattle()
    {
        // TODO This maybe should get moved to DeferredChangeScene
        if (CurrentScene.HasMeta("DestroyOnChange") && !CurrentScene.GetMeta("DestroyOnChange").AsBool())
        {
            BackgroundScene = CurrentScene;
            if (BackgroundScene is CanvasItem c)
                c.Hide();
            BackgroundScene.ProcessMode = ProcessModeEnum.Disabled;
        }
        else
        {
            CurrentScene.QueueFree();
        }
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

    private void PopCurrentScene()
    {
        if (BackgroundScene == null)
            return;
        
        CurrentScene.QueueFree();
        CurrentScene = BackgroundScene;
        BackgroundScene = null;
        GetTree().CurrentScene = CurrentScene;
        CurrentScene.ProcessMode = ProcessModeEnum.Inherit;
        if (CurrentScene is CanvasItem c)
            c.Show();
    }
}