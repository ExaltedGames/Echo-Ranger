using System.Text.Json;
using System.Text.Json.Serialization;

namespace HackmonInternals;

public static class HackmonManager
{
    public static List<HackmonMove> MoveRegistry = new();
    
    public static void LoadMoveList()
    {
        var jsonOpts = new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Moves");

        foreach (string file in Directory.EnumerateFiles(dataPath))
        {
            string json = File.ReadAllText(file);

            try
            {
                HackmonMove? parsedMove = JsonSerializer.Deserialize<HackmonMove>(json, jsonOpts);
                if(parsedMove != null) MoveRegistry.Add(parsedMove);

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error when parsing move {file}. Skipped.");
            }
        }
    }
    
}