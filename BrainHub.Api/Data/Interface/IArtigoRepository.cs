using BrainHub.Api.Application.Dtos;

namespace BrainHub.Api.Data.Interface
{
    public interface IArtigoRepository
    {
        Task<ArtigoDetailsDto> CreateArtigo(CreateArtigoDto artigoDto, int autorId);
        Task<ArtigoDetailsDto?> GetArtigoById(int id);
        Task<List<ArtigoListDto>> GetArtigosList();
    }
}
