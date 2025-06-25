using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using static MonteCarloApp.MainForm;

namespace MonteCarloApp
{
    public static class DataSaver
    {
        public static void SaveToJson(List<ResultEntry> results, string path)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(path, JsonSerializer.Serialize(results, options));
        }

        public static void SaveToCsv(List<ResultEntry> results, string path)
        {
            using var writer = new StreamWriter(path);
            writer.WriteLine("N,segment_area");
            foreach (var r in results)
                writer.WriteLine($"{r.N},{r.SegmentArea}");
        }
    }
}
