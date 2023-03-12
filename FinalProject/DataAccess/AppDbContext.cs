using FinalProject.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FinalProject.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<AuthorGenres> AuthorGenres { get; set;}

        //public DbSet<SelectListItem> AuthorList { get; set; }

        //public DbSet<SelectListItem> GenreList { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<Book>()
                .HasOne(b => b.AuthorList)
                .WithMany();

            modelBuilder.Entity<Book>()
                .HasOne(b => b.GenreList)
                .WithMany();*/
            
            modelBuilder.Entity<Author>()
                .HasMany(a => a.Books)
                .WithOne(b => b.Author);

            modelBuilder.Entity<Genre>()
                .HasMany(g => g.Books)
                .WithOne(b => b.Genre);

           modelBuilder.Entity<AuthorGenres>().HasKey(pk => new { pk.AuthorId, pk.GenreId });

            modelBuilder.Entity<AuthorGenres>()
                .HasOne(x => x.Author)
                .WithMany(x => x.AuthorGenres)
                .HasForeignKey(x => x.AuthorId);

            modelBuilder.Entity<AuthorGenres>()
                .HasOne(x => x.Genre)
                .WithMany(x => x.AuthorGenres)
                .HasForeignKey(x => x.GenreId);
        }


    }
}
