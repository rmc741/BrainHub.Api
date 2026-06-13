using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrainHub.Api.Domain.EntityConfiguration
{
    public class ComentarioConfiguration : IEntityTypeConfiguration<Comentario>
    {
        public void Configure(EntityTypeBuilder<Comentario> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Conteudo)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(c => c.DataCriacao);

            builder.HasOne(c => c.Artigo)
                .WithMany(a => a.Comentarios)
                .HasForeignKey(c => c.ArtigoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Autor)
                .WithMany(u => u.Comentarios)
                .HasForeignKey(c => c.AutorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
