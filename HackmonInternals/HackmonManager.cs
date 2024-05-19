using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using HackmonInternals.Attributes;
using HackmonInternals.Battle;
using HackmonInternals.Enums;
using HackmonInternals.Models;
using HackmonInternals.StatusEffects;
using TurnBasedBattleSystem;
using Status = HackmonInternals.StatusEffects.Status;

namespace HackmonInternals;

public static class HackmonManager
{
    public static Dictionary<int, HackmonMove> MoveRegistry { get; private set; } = new();
    // TODO: Make hackmon use IDs too
    public static Dictionary<int, HackmonData> HackmonRegistry { get; private set; } = new();

    public static Dictionary<HackmonType, Dictionary<HackmonType, float>> ElementInteractionsRegistry { get; private set; } = new();
    
    private delegate Status StatusInitializer(HackmonInstance unit, int stacks);
    private static Dictionary<string, StatusInitializer> statusMap = new();
    private static readonly JsonSerializerOptions _jsonOpts = new()
    {
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public static void LoadAllData()
    {
        // TODO: adjust move loading
        var moves = LoadData<HackmonMove>("Moves");
        foreach (var move in moves)
        {
            foreach (var status in move.TargetStatuses)
            {
                var statusType = ResolveStatusName(status.Name);
                if (statusType != null)
                {
                    move.TargetStatusTypes.Add(statusType);
                }
            }

            foreach (var status in move.UserStatuses)
            {
                var statusType = ResolveStatusName(status.Name);
                if (statusType != null)
                {
                    move.UserStatusTypes.Add(statusType);
                }
            }
            
            MoveRegistry.Add(move.ID, move);
        }
        
        var hackmonList = LoadData<HackmonData>("Hackmon");
        foreach (HackmonData d in hackmonList)
        {
            HackmonRegistry.Add(d.ID, d);
        }
        
        LoadStaticStatuses(Assembly.GetExecutingAssembly());
        
        var elementJson = File.ReadAllText("Data/ElementInteractions.json");
        
        var reg = JsonSerializer.Deserialize<Dictionary<HackmonType, Dictionary<HackmonType, float>>>(elementJson);
        ElementInteractionsRegistry = reg ?? throw new Exception("Null element registry");
    }

    public static Status InstanceStatus(string status, HackmonInstance unit, int numTurns)
    {
        if (!statusMap.ContainsKey(status)) throw new Exception($"No such status currently loaded: {status}");

        var s = statusMap[status](unit, numTurns);
        s.Name = status;
        return s;
    }

    public static void LoadStaticStatuses(Assembly a)
    {
        foreach (var type in a.GetTypes())
        {
            foreach (var method in type.GetRuntimeMethods())
            {
                if (method.IsStatic == false) continue;

                var attr = method.GetCustomAttribute<StatusAttribute>();

                if (attr == null) continue;

                StatusInitializer del;
                try
                {
                    del = (StatusInitializer)method.CreateDelegate(typeof(StatusInitializer));
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to load status {attr.Name}, incorrect method signature.");
                    continue;
                }

                statusMap[attr.Name] = del;
                Console.WriteLine($"Found and loaded status with name {attr.Name}");
            }
        }
    }

    public static void StartBattle(TrainerData playerData, TrainerData enemyData)
    {
        HackmonBattleManager.StartBattle(playerData.CurrentParty, enemyData.CurrentParty);
    }
    
    private static Type? ResolveStatusName(string statusName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var statusType = assembly.GetType($"HackonInternals.StatusEffects.{statusName}");

        return statusType;
    }

    private static List<T> LoadData<T>(string dir)
    {
        var dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Data/{dir}");
        List<T> loadedData = new();

        foreach (var file in Directory.EnumerateFiles(dataPath, "*.json", SearchOption.AllDirectories))
        {
            var json = File.ReadAllText(file);
            try
            {
                var parsedItem = JsonSerializer.Deserialize<T>(json, _jsonOpts);
                if (parsedItem != null) loadedData.Add(parsedItem);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error when parsing {file}. Skipped.");
            }
        }

        return loadedData;
    }
}