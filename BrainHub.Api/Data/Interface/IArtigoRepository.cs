using BrainHub.Api.Application.Dtos;
using BrainHub.Api.Domain;

namespace BrainHub.Api.Data.Interface
{
    public interface IArtigoRepository
    {
        Task<Artigo> CreateArtigo(CreateArtigoDto artigoDto);
        Task<List<Artigo>> GetArtigosList();
    }
}