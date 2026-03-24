using DM_Meeting_5.Models;
using DM_Meeting_5.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_Meeting_5.Data
{
    public class GamesContext : DbContext
    {
        public GamesContext(DbContextOptions<GamesContext> options) :
            base(options){
            //Database.EnsureCreated();
        }
        public DbSet<Country> Countries { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Studio> Studios { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<GameFullInfo> GameFullInfos { get; set; }

        public int GetGamesQuantityByStyle(GameStyle style) => 
            throw new NotSupportedException();

        public double GetAverageGamesQuantity()=>
            throw new NotSupportedException();

        public IQueryable<GamesQuantityDTO> GetGamesQuantityByStudio()=>
            FromExpression(()=>GetGamesQuantityByStudio());

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudiosByCountry>().HasNoKey();
            modelBuilder.Entity<GameFullInfo>()
                .HasNoKey()
                .ToView("GameFullInfo");
            modelBuilder.Entity<GamesQuantityDTO>().
                HasNoKey();
            modelBuilder.HasDbFunction(typeof(GamesContext).GetMethod(nameof(GetGamesQuantityByStyle), [typeof(GameStyle)]))
                .HasName("getGamesQuantityByStyle");
            modelBuilder.HasDbFunction(() => GetAverageGamesQuantity())
                .HasName("getAverageGamesQuantity");
            modelBuilder.HasDbFunction(() => GetGamesQuantityByStudio())
                .HasName("getGamesQuantityByStudio");
        }
    }
}
