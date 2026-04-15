namespace BrainHub.Api.Domain
{
    public class Artigo
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string? Resumo { get; set; }

        public string Conteudo { get; set; } = string.Empty;

        public int AutorId { get; set; }

        public Usuario Autor { get; set; } = null!;

        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
