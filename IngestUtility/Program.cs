﻿using System;
using System.IO;
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
            var getSheetService = GetSheetService.Default(apiKey, sheetId, "Housewares");
            var service = new CommunitySheetIngestService(getSheetService);

            var result = await service.Ingest();

            await File.WriteAllTextAsync(outputPath, JsonSerializer.Serialize(result));
        }
    }
}
