using Microsoft.EntityFrameworkCore;
using PhotoGalleryAPI.DAL.Entities;

namespace PhotoGalleryAPI.DAL.DBContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options, bool isTest = false) : base(options)
        {
            if (isTest)
                Database.EnsureCreated();
            else
                Database.Migrate();
        }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(x =>
            {
                x.HasIndex(y => y.Username).IsUnique();
                x.HasIndex(y => y.Email).IsUnique();
                x.Property(y => y.Username).HasColumnType("varchar").HasMaxLength(60);
                x.Property(y => y.Email).HasColumnType("varchar").HasMaxLength(60);
            });

            modelBuilder.Entity<Photo>(x =>
            {
                x.Property(y => y.Filename).HasColumnType("varchar").HasMaxLength(60);
            });

            modelBuilder.Entity<Role>(x =>
            {
                x.Property(y => y.RoleName).HasColumnType("varchar").HasMaxLength(60);
            });

            modelBuilder.Entity<Album>(x =>
            {
                x.Property(y => y.Title).HasColumnType("varchar").HasMaxLength(60);
            });

            modelBuilder.Entity<Person>(x =>
            {
                x.HasIndex(y => y.Username).IsUnique();
                x.HasIndex(y => y.Email).IsUnique();
                x.Property(y => y.Username).HasColumnType("varchar").HasMaxLength(60);
                x.Property(y => y.Email).HasColumnType("varchar").HasMaxLength(60);
            });

            modelBuilder.Entity<Album>()
                .HasMany(a => a.Photos)
                .WithOne(p => p.Album)
                .HasForeignKey(p => p.AlbumId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Person>()
                .HasMany(p => p.Albums)
                .WithOne(ph => ph.CreatedBy)
                .HasForeignKey(ph => ph.CreatedByPersonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Person>()
                .HasOne(p => p.Role)
                .WithMany(r => r.Persons)
                .HasForeignKey(p => p.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Photo)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PhotoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Person)
                .WithMany()
                .HasForeignKey(l => l.PersonId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
