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

        public async Task<List<Artigo>> GetArtigosList()
        {
            return await _context.Artigos
                .AsNoTracking()
                .OrderByDescending(a => a.DataCriacao)
                .ToListAsync();
        }
    }
}
