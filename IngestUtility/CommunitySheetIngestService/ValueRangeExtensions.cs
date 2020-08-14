using System;
using System.Collections.Generic;
using System.Linq;
using Core.Catalogue;
using Core.Shared;
using Google.Apis.Sheets.v4.Data;

namespace IngestUtility.CommunitySheetIngestService
{
    public static class ValueRangeExtensions
    {
        public class HeadersNotFoundException : BaseException { }

        public static List<Dictionary<string, Object>> ToListDictionary(this ValueRange valueRange)
        {
            var rows = valueRange.Values;

            IList<string> headers = rows.First().Select(_ => _.ToString()).ToList();

            if (headers == null)
            {
                throw new HeadersNotFoundException();
            }

            return rows.Skip(1)
                .Select(values => headers
                    .Zip(values)
                    .ToDictionary(_ => _.First, _ => _.Second)
                )
                .ToList();
        }

        public static List<Item> ToListItem(this ValueRange valueRange)
        {
            return new List<Item>();
        }
    }
}