namespace BrainHub.Api.Application.Dtos
{
    public class AuthResponseDto
    {
        public int UsuarioId { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public DateTime ExpiraEm { get; set; }
    }
}
