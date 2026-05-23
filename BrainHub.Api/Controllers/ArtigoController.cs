using BrainHub.Api.Application.Dtos;
using BrainHub.Api.Data.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [Authorize]
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

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var autorIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(autorIdClaim, out var autorId))
            {
                return Unauthorized(new { message = "Token de usuario invalido." });
            }

            try
            {
                var result = await _artigoRepository.CreateArtigo(artigoDto, autorId);
                return CreatedAtAction(nameof(GetArtigoById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
