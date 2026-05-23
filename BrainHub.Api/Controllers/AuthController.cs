using BrainHub.Api.Application.Dtos;
using BrainHub.Api.Config;
using BrainHub.Api.Data;
using BrainHub.Api.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BrainHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BrainHubDbContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly JwtConfig _jwtConfig;

        public AuthController(
            BrainHubDbContext context,
            IPasswordHasher<Usuario> passwordHasher,
            IOptions<JwtConfig> jwtOptions)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtConfig = jwtOptions.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUsuarioDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
            {
                ModelState.AddModelError(nameof(dto.Nome), "Nome e obrigatorio.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                ModelState.AddModelError(nameof(dto.Email), "Email e obrigatorio.");
            }

            if (string.IsNullOrWhiteSpace(dto.Senha))
            {
                ModelState.AddModelError(nameof(dto.Senha), "Senha e obrigatoria.");
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var email = dto.Email.Trim().ToLower();
            var emailJaExiste = await _context.Usuarios.AnyAsync(u => u.Email == email);

            if (emailJaExiste)
            {
                return BadRequest(new { message = "Email ja cadastrado." });
            }

            var usuario = new Usuario
            {
                Nome = dto.Nome.Trim(),
                Email = email
            };

            usuario.PasswordHash = _passwordHasher.HashPassword(usuario, dto.Senha);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(CreateAuthResponse(usuario));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUsuarioDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                ModelState.AddModelError(nameof(dto.Email), "Email e obrigatorio.");
            }

            if (string.IsNullOrWhiteSpace(dto.Senha))
            {
                ModelState.AddModelError(nameof(dto.Senha), "Senha e obrigatoria.");
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var email = dto.Email.Trim().ToLower();
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.Ativo);

            if (usuario is null)
            {
                return Unauthorized(new { message = "Email ou senha invalidos." });
            }

            var passwordResult = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, dto.Senha);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { message = "Email ou senha invalidos." });
            }

            return Ok(CreateAuthResponse(usuario));
        }

        private AuthResponseDto CreateAuthResponse(Usuario usuario)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpirationInMinutes);

            return new AuthResponseDto
            {
                UsuarioId = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Token = GenerateToken(usuario, expiresAt),
                ExpiraEm = expiresAt
            };
        }

        private string GenerateToken(Usuario usuario, DateTime expiresAt)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
