using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM_Meeting_5.Data
{
    public class GamesContextFactory : IDesignTimeDbContextFactory<GamesContext>
    {
        public GamesContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<GamesContext> optionsBuilder = 
                new DbContextOptionsBuilder<GamesContext>();
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appSettings.json");
            IConfigurationRoot configurationRoot = configurationBuilder.Build();
            string connStr = configurationRoot.GetConnectionString("Default") ??
                throw new InvalidOperationException("You should provide default connection string!");
            optionsBuilder.UseSqlServer(connStr);
            optionsBuilder.LogTo(str=>Debug.WriteLine(str));
            DbContextOptions<GamesContext> options = optionsBuilder.Options;
            GamesContext gamesContext = new GamesContext(options);
            return gamesContext;
        }
    }
}
