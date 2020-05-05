using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Mappings
{
    public class EscolaConfiguration : IEntityTypeConfiguration<Escola>
    {
        public void Configure(EntityTypeBuilder<Escola> builder)
        {
            builder
                .HasMany(e => e.Turmas)
                .WithOne(t => t.Escola)
                .HasForeignKey(t => t.EscolaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}