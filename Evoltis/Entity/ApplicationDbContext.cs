using Evoltis.Models;
using Microsoft.EntityFrameworkCore;

namespace Evoltis.Entity
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Club> Clubs { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Club>()
                .HasKey(c => c.IdClub);

            modelBuilder.Entity<Club>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Club>()
                .Property(c => c.CUIT)
                .IsRequired()
                .HasMaxLength(11);

            modelBuilder.Entity<Club>()
                .Property(c => c.Address)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Club>()
                .Property(c => c.StadiumName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Club>()
                .HasOne(c => c.Tournament)
                .WithMany()
                .HasForeignKey(c => c.IdTournament);

            modelBuilder.Entity<Club>()
                .Property(c => c.Active)
                .IsRequired();

            modelBuilder.Entity<Tournament>()
                 .HasKey(t => t.IdTournament);

            modelBuilder.Entity<Tournament>()
                .Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

        }
    }
}
