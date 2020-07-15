using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SuperCyklisterneAPI.Entities
{
    public partial class SupercyklisterneContext : DbContext
    {
        public SupercyklisterneContext()
        {
        }

        public SupercyklisterneContext(DbContextOptions<SupercyklisterneContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Medlemmer> Medlemmer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Medlemmer>(entity =>
            {
                entity.HasKey(e => e.MedlemsId)
                    .HasName("PK__Medlemmer__6FA2FB108DA67876");

                entity.ToTable("Medlemmer");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Kodeord)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Navn)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
