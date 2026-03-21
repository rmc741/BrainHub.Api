using BrainHub.Api.Application.Dtos;
using BrainHub.Api.Data.Interface;
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetArtigoById(int id)
        {
            var result = await _artigoRepository.GetArtigoById(id);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateArtigo([FromBody] CreateArtigoDto artigoDto)
        {
            if (string.IsNullOrWhiteSpace(artigoDto.Titulo))
            {
                ModelState.AddModelError(nameof(artigoDto.Titulo), "Titulo e obrigatorio.");
            }

            if (string.IsNullOrWhiteSpace(artigoDto.Conteudo))
            {
                ModelState.AddModelError(nameof(artigoDto.Conteudo), "Conteudo e obrigatorio.");
            }

            if (string.IsNullOrWhiteSpace(artigoDto.Autor))
            {
                ModelState.AddModelError(nameof(artigoDto.Autor), "Autor e obrigatorio.");
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var result = await _artigoRepository.CreateArtigo(artigoDto);
            return CreatedAtAction(nameof(GetArtigoById), new { id = result.Id }, result);
        }
    }
}
