using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using HackmonInternals.Battle;
using HackmonInternals.Models;
using HackmonInternals.StatusEffects;

namespace HackmonInternals;

public static class HackmonManager
{
    public static Dictionary<int, HackmonMove> MoveRegistry { get; private set; } = new();
    
    // TODO: Make hackmon use IDs too
    public static List<HackmonData> HackmonRegistry { get; private set; } = new();

    private static readonly JsonSerializerOptions _jsonOpts = new()
    {
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public static void LoadAllData()
    {
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
        
        HackmonRegistry = LoadData<HackmonData>("Hackmon");
    }

    public static BattleManager StartBattle(TrainerData playerData, TrainerData enemyData)
    {
        return new BattleManager(playerData, enemyData);
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

        foreach (var file in Directory.EnumerateFiles(dataPath))
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