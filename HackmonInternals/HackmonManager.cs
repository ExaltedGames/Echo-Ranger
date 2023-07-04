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
            foreach (var statusName in move.TargetStatuses)
            {
                var status = ResolveStatusName(statusName);
                if (status != null)
                {
                    move.TargetStatusList.Add(status);
                }
            }

            foreach (var statusName in move.UserStatuses)
            {
                var status = ResolveStatusName(statusName);
                if (status != null)
                {
                    move.UserStatusList.Add(status);
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

    private static Status? ResolveStatusName(string statusName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resolvedStatus =
            Activator.CreateInstance(null!, $"{typeof(Status).Namespace}.{statusName}")
                ?.Unwrap() as Status;

        return resolvedStatus;
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