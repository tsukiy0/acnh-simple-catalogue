using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Shared;
using IngestUtility.IngestService;

namespace IngestUtility
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configService = new EnvironmentConfigService();
            var apiKey = configService.Get("GOOGLE_API_KEY");
            var sheetId = configService.Get("COMMUNITY_SHEET_ID");
            var outputPath = configService.Get("OUTPUT_PATH");
            var getSheeetServices = new List<string> {
                "Housewares",
                "Miscellaneous",
                "Wall-mounted",
                "Wallpaper",
                "Floors",
                "Rugs",
                "Photos",
                "Posters",
                "Art",
                "Fossils",
                "Music",
             }.Select(_ => (IGetSheetService)GetSheetService.Default(apiKey, sheetId, _)).ToList();
            var service = new CommunitySheetIngestService(getSheeetServices);

            var result = await service.Ingest();

            await File.WriteAllTextAsync(outputPath, JsonSerializer.Serialize(result));
        }
    }
}
