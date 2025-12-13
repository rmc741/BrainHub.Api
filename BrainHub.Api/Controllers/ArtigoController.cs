using BrainHub.Api.Data.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrainHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtigoController : ControllerBase
    {
        private readonly IArtigoRepository _artigoRepository;

        public ArtigoController(IArtigoRepository artigoRepository)
        {
            _artigoRepository = artigoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetArtigos()
        {
            var result = await _artigoRepository.GetArtigosList();
            return Ok(result);
        }
    }
}
