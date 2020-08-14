using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.GetRequest;

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

        public static GetSheetService Default(string apiKey, string sheetId, string sheetRange)
        {
            var sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
            });
            return new GetSheetService(sheetsService, sheetId, sheetRange);
        }

        public async Task<IList<IList<object>>> Get()
        {
            var request = sheetsService.Spreadsheets.Values
                .Get(sheetId, sheetRange);

            request.ValueRenderOption = ValueRenderOptionEnum.FORMULA;

            var result = await request.ExecuteAsync();

            return result.Values;
        }
    }
}