namespace BrainHub.Api.Application.Dtos
{
    public class ComentarioDto
    {
        public int Id { get; set; }

        public string Conteudo { get; set; } = string.Empty;

        public string Autor { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; }
    }
}
