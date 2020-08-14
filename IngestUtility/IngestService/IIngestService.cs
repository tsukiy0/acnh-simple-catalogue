using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Catalogue;

namespace IngestUtility.IngestService
{
    public interface IIngestService
    {
        Task<IList<Item>> Ingest();
    }
}