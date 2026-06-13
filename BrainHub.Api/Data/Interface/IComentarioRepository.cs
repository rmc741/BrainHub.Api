using BrainHub.Api.Application.Dtos;

namespace BrainHub.Api.Data.Interface
{
    public interface IComentarioRepository
    {
        Task<List<ComentarioDto>> GetComentariosByArtigoId(int artigoId);
        Task<ComentarioDto> CreateComentario(int artigoId, CreateComentarioDto comentarioDto, int autorId);
    }
}
