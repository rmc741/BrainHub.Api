using BrainHub.Api.Data.Interface;
using BrainHub.Api.Domain;

namespace BrainHub.Api.Data.Repository
{
    public class ArtigoRepository : IArtigoRepository
    {
        public Task<List<Artigo>> GetArtigosList()
        {
            throw new NotImplementedException();
        }
    }
}
