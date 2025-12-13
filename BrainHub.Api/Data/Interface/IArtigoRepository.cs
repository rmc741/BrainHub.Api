using BrainHub.Api.Domain;

namespace BrainHub.Api.Data.Interface
{
    public interface IArtigoRepository
    {
        Task<List<Artigo>> GetArtigosList();
    }
}