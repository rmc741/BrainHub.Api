namespace BrainHub.Api.Domain
{
    public class Comentario
    {
        public int Id { get; set; }

        public string Conteudo { get; set; } = string.Empty;

        public int ArtigoId { get; set; }

        public Artigo Artigo { get; set; } = null!;

        public int AutorId { get; set; }

        public Usuario Autor { get; set; } = null!;

        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}
