namespace BrainHub.Api.Application.Dtos
{
    public class CreateArtigoDto
    {
        public string Titulo { get; set; } = string.Empty;

        public string? Resumo { get; set; }

        public string Conteudo { get; set; } = string.Empty;

        public int AutorId { get; set; }
    }
}
