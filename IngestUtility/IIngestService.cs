using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Catalogue;

namespace IngestUtility
{
    public interface IIngestService
    {
        Task<IList<Item>> Ingest();
    }
}