namespace BrainHub.Api.Domain
{
    public class Artigo
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Conteudo { get; set; }

        public string Autor { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
