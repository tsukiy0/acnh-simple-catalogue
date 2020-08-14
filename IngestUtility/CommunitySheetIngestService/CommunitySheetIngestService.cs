using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Catalogue;
using Google.Apis.Sheets.v4;

namespace IngestUtility.CommunitySheetIngestService
{
    public class CommunitySheetIngestService : IIngestService
    {
        private readonly SheetsService sheetsService;
        private readonly string sheetId;
        private readonly string sheetRange;

        public CommunitySheetIngestService(SheetsService sheetsService, string sheetId, string sheetRange)
        {
            this.sheetsService = sheetsService;
            this.sheetId = sheetId;
            this.sheetRange = sheetRange;
        }

        public async Task<IList<Item>> Ingest()
        {
            var rows = sheetsService.Spreadsheets.Values
                .Get(sheetId, sheetRange)
                .Execute()
                .Values;


            foreach (var row in rows)
            {
                Console.WriteLine(row);
            }


            return new List<Item>();
        }
    }

}