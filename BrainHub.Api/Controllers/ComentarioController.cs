using BrainHub.Api.Application.Dtos;
using BrainHub.Api.Data.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BrainHub.Api.Controllers
{
    [Route("api/artigo/{artigoId:int}/comentarios")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly IComentarioRepository _comentarioRepository;

        public ComentarioController(IComentarioRepository comentarioRepository)
        {
            _comentarioRepository = comentarioRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetComentarios(int artigoId)
        {
            var result = await _comentarioRepository.GetComentariosByArtigoId(artigoId);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComentario(
            int artigoId,
            [FromBody] CreateComentarioDto comentarioDto)
        {
            if (string.IsNullOrWhiteSpace(comentarioDto.Conteudo))
            {
                ModelState.AddModelError(nameof(comentarioDto.Conteudo), "Conteudo e obrigatorio.");
            }
            else if (comentarioDto.Conteudo.Trim().Length > 2000)
            {
                ModelState.AddModelError(
                    nameof(comentarioDto.Conteudo),
                    "Conteudo deve ter no maximo 2000 caracteres.");
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
                var result = await _comentarioRepository.CreateComentario(
                    artigoId,
                    comentarioDto,
                    autorId);

                return Created(string.Empty, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
