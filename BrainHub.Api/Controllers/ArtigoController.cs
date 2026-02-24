using BrainHub.Api.Application.Dtos;
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

        [HttpPost]
        public async Task<IActionResult> CreateArtigo([FromBody] CreateArtigoDto artigoDto) {
            var result = await _artigoRepository.CreateArtigo(artigoDto);
            return Ok();
        }
    }
}
