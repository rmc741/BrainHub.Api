using BrainHub.Api.Application.Dtos;
using BrainHub.Api.Data.Interface;
using BrainHub.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace BrainHub.Api.Data.Repository
{
    public class ComentarioRepository : IComentarioRepository
    {
        private readonly BrainHubDbContext _context;

        public ComentarioRepository(BrainHubDbContext context)
        {
            _context = context;
        }

        public async Task<List<ComentarioDto>> GetComentariosByArtigoId(int artigoId)
        {
            return await _context.Comentarios
                .AsNoTracking()
                .Where(c => c.ArtigoId == artigoId)
                .OrderBy(c => c.DataCriacao)
                .Select(c => new ComentarioDto
                {
                    Id = c.Id,
                    Conteudo = c.Conteudo,
                    Autor = c.Autor.Nome,
                    DataCriacao = c.DataCriacao
                })
                .ToListAsync();
        }

        public async Task<ComentarioDto> CreateComentario(
            int artigoId,
            CreateComentarioDto comentarioDto,
            int autorId)
        {
            var artigoExiste = await _context.Artigos
                .AsNoTracking()
                .AnyAsync(a => a.Id == artigoId);

            if (!artigoExiste)
            {
                throw new KeyNotFoundException("Artigo nao encontrado.");
            }

            var usuario = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == autorId && u.Ativo);

            if (usuario is null)
            {
                throw new InvalidOperationException("Usuario autenticado nao foi encontrado ou esta inativo.");
            }

            var comentario = new Comentario
            {
                Conteudo = comentarioDto.Conteudo.Trim(),
                ArtigoId = artigoId,
                AutorId = autorId
            };

            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();

            return new ComentarioDto
            {
                Id = comentario.Id,
                Conteudo = comentario.Conteudo,
                Autor = usuario.Nome,
                DataCriacao = comentario.DataCriacao
            };
        }
    }
}
