using System;
using System.Collections.Generic;
using System.Linq;
using Core.Shared;

namespace IngestUtility.IngestService
{
    public static class SheetsExtensions
    {
        public class HeadersNotFoundException : BaseException { }

        public static List<Dictionary<string, Object>> ToListDictionary(this IList<IList<object>> rows)
        {
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
    }
}