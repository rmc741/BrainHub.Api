using BrainHub.Api.Application.Dtos;
using BrainHub.Api.Data.Interface;
using BrainHub.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace BrainHub.Api.Data.Repository
{
    public class ArtigoRepository : IArtigoRepository
    {
        private readonly BrainHubDbContext _context;

        public ArtigoRepository(BrainHubDbContext context)
        {
            _context = context;
        }

        public async Task<List<ArtigoListDto>> GetArtigosList()
        {
            return await _context.Artigos
                .AsNoTracking()
                .OrderByDescending(a => a.DataCriacao)
                .Select(a => new ArtigoListDto
                {
                    Id = a.Id,
                    Titulo = a.Titulo,
                    Resumo = a.Resumo,
                    DataPublicacao = a.DataCriacao
                })
                .ToListAsync();
        }

        public async Task<ArtigoDetailsDto?> GetArtigoById(int id)
        {
            return await _context.Artigos
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new ArtigoDetailsDto
                {
                    Id = a.Id,
                    Titulo = a.Titulo,
                    Resumo = a.Resumo,
                    Conteudo = a.Conteudo,
                    Autor = a.Autor.Nome,
                    DataPublicacao = a.DataCriacao
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ArtigoDetailsDto> CreateArtigo(CreateArtigoDto artigoDto)
        {
            var usuario = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == artigoDto.AutorId);

            if (usuario is null)
            {
                throw new InvalidOperationException("Usuario informado nao foi encontrado.");
            }

            var artigo = new Artigo
            {
                Titulo = artigoDto.Titulo.Trim(),
                Resumo = string.IsNullOrWhiteSpace(artigoDto.Resumo) ? null : artigoDto.Resumo.Trim(),
                Conteudo = artigoDto.Conteudo.Trim(),
                AutorId = artigoDto.AutorId
            };

            _context.Artigos.Add(artigo);
            await _context.SaveChangesAsync();

            return new ArtigoDetailsDto
            {
                Id = artigo.Id,
                Titulo = artigo.Titulo,
                Resumo = artigo.Resumo,
                Conteudo = artigo.Conteudo,
                Autor = usuario.Nome,
                DataPublicacao = artigo.DataCriacao
            };
        }
    }
}
