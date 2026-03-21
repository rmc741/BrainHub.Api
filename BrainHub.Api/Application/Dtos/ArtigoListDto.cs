namespace BrainHub.Api.Application.Dtos
{
    public class ArtigoListDto
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string? Resumo { get; set; }

        public DateTime DataPublicacao { get; set; }
    }
}
