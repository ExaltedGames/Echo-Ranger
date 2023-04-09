using System.Text.Json;
using System.Text.Json.Serialization;
using HackmonInternals.StatusEffects;

namespace HackmonInternals;

public static class HackmonManager
{
    public static List<HackmonMove> MoveRegistry = new();
    public static List<Hackmon> HackmonRegistry = new();

    private static JsonSerializerOptions _jsonOpts = new JsonSerializerOptions
    {
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public static void LoadAllData()
    {
        MoveRegistry = LoadData<HackmonMove>("Moves");
        foreach (HackmonMove move in MoveRegistry)
        {
            foreach (string statusName in move.TargetStatuses)
            {
                Status? status = ResolveStatusName(statusName);
                if (status != null)
                {
                    move.TargetStatusList.Add(status);
                }
            }

            foreach (string statusName in move.UserStatuses)
            {
                Status? status = ResolveStatusName(statusName);
                if (status != null)
                {
                    move.UserStatusList.Add(status);
                }
            }
        }
        HackmonRegistry = LoadData<Hackmon>("Hackmon");
    }

    private static Status? ResolveStatusName(string statusName)
    {
        Status? resolvedStatus =
            Activator.CreateInstance("HackmonInternals", $"HackmonInternals.StatusEffects.{statusName}")
                ?.Unwrap() as Status;

        return resolvedStatus;
    }

    private static List<T> LoadData<T>(string dir)
    {
        string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Data/{dir}");
        List<T> loadedData = new();

        foreach (string file in Directory.EnumerateFiles(dataPath))
        {
            string json = File.ReadAllText(file);

            try
            {
                T? parsedItem = JsonSerializer.Deserialize<T>(json, _jsonOpts);
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