using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4;

namespace IngestUtility.IngestService
{
    public interface IGetSheetService
    {
        Task<IList<IList<object>>> Get();
    }

    public class GetSheetService : IGetSheetService
    {
        private readonly SheetsService sheetsService;
        private readonly string sheetId;
        private readonly string sheetRange;

        public GetSheetService(SheetsService sheetsService, string sheetId, string sheetRange)
        {
            this.sheetsService = sheetsService;
            this.sheetId = sheetId;
            this.sheetRange = sheetRange;
        }

        public async Task<IList<IList<object>>> Get()
        {
            var result = await sheetsService.Spreadsheets.Values
                .Get(sheetId, sheetRange)
                .ExecuteAsync();

            return result.Values;
        }
    }
}