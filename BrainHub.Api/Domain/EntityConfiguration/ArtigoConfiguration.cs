using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrainHub.Api.Domain.EntityConfiguration
{
    public class ArtigoConfiguration : IEntityTypeConfiguration<Artigo>
    {
        public void Configure(EntityTypeBuilder<Artigo> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Titulo)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(a => a.Conteudo)
                .IsRequired();

            builder.Property(a => a.Autor)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.DataCriacao);
        }
    }
}
