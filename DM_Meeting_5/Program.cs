// See https://aka.ms/new-console-template for more information
using DM_Meeting_5.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.Extensions.Configuration;
using DM_Meeting_5.Models;
using Microsoft.Data.SqlClient;
using DM_Meeting_5.Models.DTOs;

Console.OutputEncoding = Encoding.UTF8;
GamesContextFactory contextFactory = new GamesContextFactory();
using GamesContext context = contextFactory.CreateDbContext(args);
var someCities = context.Cities.Where(c => c.Name.Contains("Лі"));
foreach (var c in someCities)
    Console.WriteLine($"{c.Name}");
Console.WriteLine("---------");
string pattern = "%і%";
//SqlParameter sqlParameter1 = new SqlParameter("@cityNamePattern", System.Data.SqlDbType.NVarChar);
//sqlParameter1.Value = pattern;
//IQueryable<City> cities = context.Cities.FromSqlRaw("SELECT * FROM Cities WHERE Name LIKE @cityNamePattern;", sqlParameter1);
//IQueryable<City> cities = context.Cities.FromSqlRaw($"SELECT * FROM Cities WHERE Name LIKE {pattern}");
IQueryable<City> cities = context.Cities.FromSqlInterpolated($"SELECT * FROM Cities WHERE Name LIKE {pattern}")
    .Take(2)
    .OrderByDescending(t=>t.Name);
foreach (var c in cities)
    Console.WriteLine($"{c.Name}");

//SqlParameter sqlParameter2 = new SqlParameter("@cityName", System.Data.SqlDbType.NVarChar);
//sqlParameter2.Value = "Польща";
//int rows = await context.Database.ExecuteSqlRawAsync("INSERT INTO Countries VALUES(@cityName)", sqlParameter2);
//Console.WriteLine($"{rows} added!");
Console.WriteLine("----Stored Procedure example-------");
SqlParameter studioNameParam = new SqlParameter("@sName", System.Data.SqlDbType.NVarChar);
studioNameParam.Value = "Playrix";
var games = context.Games.FromSqlRaw("EXEC getGamesByStudioName @sName", studioNameParam);
foreach (Game game in games)
    Console.WriteLine($"{game.Title}, {game.GameStyle}");
/////////////////////////////////////////////////////////
Console.WriteLine("------------");
var studiosByCountries = context
    .Set<StudiosByCountry>()
    .FromSqlRaw("EXEC getStudiosQuantityByCountry");
    //.AsEnumerable()
    //.Where(t=>t.Name == "Україна");
foreach(var item in studiosByCountries)
{
    Console.WriteLine($"Кількість студій в {item.Name}: {item.StudiosCount}");
}

SqlParameter nameParam1 = new SqlParameter("@countryName", System.Data.SqlDbType.NVarChar);
nameParam1.Value = "Норвегія";
SqlParameter rowIdParam = new SqlParameter
{
    ParameterName = "@rowId",
    SqlDbType = System.Data.SqlDbType.Int,
    Direction = System.Data.ParameterDirection.Output
};
//await context.Database.ExecuteSqlRawAsync ("EXEC sp_addCountry @countryName, @rowId OUTPUT", nameParam1, rowIdParam);
//Console.WriteLine($"Додано нову країну {nameParam1.Value} з Id: {rowIdParam.Value}");
///////////////////////////
//////------------Робота з представленнями------------

var gamesDetails = context.GameFullInfos
    .Where(t => t.GameStyle == GameStyle.MultiPerson)
    .OrderBy(t => t.Studio)
    //.Skip(2)
    //.Take(4)
    ;
foreach(var game in gamesDetails)
{
    Console.WriteLine($"{game.Id}. {game.Title}. Style:{game.GameStyle}. Studio: {game.Studio}");
}

var groups = context.Games.GroupBy(t => t.StudioId)
    .Where(g => g.Count() > context.GetAverageGamesQuantity());
foreach(var group in groups)
{
    Console.WriteLine($"Назва студії: {group.Key}");
    foreach(var game in group)
    {
        Console.WriteLine($"{game.Title} StudioId: {game.StudioId}");
    }
    Console.WriteLine("-----------");
}

///////////////////////////
//////------------Робота з UDF------------
//int singlePersonGameCount = context.GetGamesQuantityByStyle(GameStyle.SinglePerson);
//int multiPlayersGameCount = context.GetGamesQuantityByStyle(GameStyle.MultiPerson);
//Console.WriteLine($"Single Person COunt: {singlePersonGameCount}");
//Console.WriteLine($"Multi Player Games Count: {multiPlayersGameCount}");
//double avg = context.GetAverageGamesQuantity();
//Console.WriteLine($"Agerage games: {avg}");
var gamesQuantityDetails = context.GetGamesQuantityByStudio();
foreach(var details in gamesQuantityDetails)
{
    Console.WriteLine($"Студія {details.Name} випускає {details.GamesQuantity} ігор");
}
async Task SeedCounties(GamesContext context)
{
    if (!context.Countries.Any())
    {
        Country country1 = new Country { Name = "Україна" };
        Country country2 = new Country { Name = "Швеція" };
        Country country3 = new Country { Name = "Великобританія" };
        await context.Countries.AddRangeAsync(country1, country2, country3);
        await context.SaveChangesAsync();
        Console.WriteLine("Країни були додані!");
    }
}




async Task SeedData(GamesContext context)
{
    try
    {
        Country country1 = await context.Countries.FirstAsync(t => t.Name == "Україна");
        Country country2 = await context.Countries.FirstAsync(t => t.Name == "Швеція");
        Country country3 = await context.Countries.FirstAsync(t => t.Name == "Великобританія");
        City city1 = new City { Name = "Київ", Country = country1 };
        City city8 = new City { Name = "Львів", Country = country1 };
        City city2 = new City { Name = "Кривий Ріг", Country = country1 };
        City city3 = new City { Name = "Стокгольм", Country = country2 };
        City city9 = new City { Name = "Уппсала", Country = country2 };
        City city4 = new City { Name = "Лондон", Country = country3 };
        City city5 = new City { Name = "Хай Уіком", Country = country3 };
        City city6 = new City { Name = "Манчестер", Country = country3 };
        City city7 = new City { Name = "Ліверпуль", Country = country3 };
        await context.AddRangeAsync(city1, city2, city3, city4, city5, city6,
            city7, city8, city9);
        await context.SaveChangesAsync();
        Studio studio1 = new Studio { Name = "4A Games" };
        Studio studio6 = new Studio { Name = "Playrix" };
        Studio studio2 = new Studio { Name = "Tencent" };
        Studio studio3 = new Studio { Name = "Nintendo" };
        Studio studio4 = new Studio { Name = "Take-Two Interactive" };
        Studio studio5 = new Studio { Name = "Room 8 Group" };
        studio1.Cities.Add(city1);
        studio1.Cities.Add(city8);
        studio2.Cities.Add(city2);
        studio2.Cities.Add(city8);
        studio3.Cities.Add(city4);
        studio3.Cities.Add(city6);
        studio3.Cities.Add(city9);
        studio4.Cities.Add(city5);
        studio5.Cities.Add(city4);
        studio5.Cities.Add(city6);
        studio5.Cities.Add(city1);
        studio5.Cities.Add(city2);
        studio5.Cities.Add(city8);

        await context.Studios.AddRangeAsync(studio1, studio2, studio3, studio4, studio5, studio6);
        await context.SaveChangesAsync();

        Game game1 = new Game { Title = "GTA 6", Studio = studio2, GameStyle = GameStyle.SinglePerson };
        Game game2 = new Game { Title = "STALKER", Studio = studio1, GameStyle = GameStyle.MultiPerson };
        Game game3 = new Game { Title = "Crimson Desert", Studio = studio3, GameStyle = GameStyle.SinglePerson };
        Game game4 = new Game { Title = "Little Nightmares 3", Studio = studio4, GameStyle = GameStyle.MultiPerson };
        Game game5 = new Game { Title = "Fable ", Studio = studio5, GameStyle = GameStyle.MultiPerson };
        Game game6 = new Game { Title = "Forza Horizon 6", Studio = studio6, GameStyle = GameStyle.SinglePerson };
        Game game7 = new Game { Title = "Death Stranding 2", Studio = studio2, GameStyle = GameStyle.SinglePerson };
        Game game8 = new Game { Title = "Resident Evil: Requiem", Studio = studio4, GameStyle = GameStyle.MultiPerson };
        await context.Games.AddRangeAsync(game1, game2, game3, game4, game5, game6,
            game7, game8);
        await context.SaveChangesAsync();
        Console.WriteLine("Дані збережені успішно!");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

async Task LoadingExamples()
{
    using (GamesContext context = contextFactory.CreateDbContext(args))
    {
        //// 1
        //var cities =  await context.Cities
        //    .Include(t => t.Country).ToListAsync();
        //cities = cities.Where(t => t.CountryId == 1).ToList();

        Console.WriteLine("-----Приклад неявного (implicit) завантаження------");
        //// 2
        var cities = context.Cities
            .Include(t => t.Country).Where(t => t.CountryId == 1);
        foreach (var city in cities)
            Console.WriteLine($"{city.Name}, {city.Country.Name}");
    }
    using (GamesContext context1 = contextFactory.CreateDbContext(args))
    {
        Console.WriteLine("-----Приклад явного (explicit) завантаження------");
        City kyivCity = await context1.Cities.FirstAsync(t => t.Name == "Київ");
        await context1.Entry(kyivCity)
            .Reference(t => t.Country)
            .LoadAsync();
        await context1.Entry(kyivCity)
            .Collection(t => t.Studios)
            .LoadAsync();
        Console.WriteLine($"Студії в м. {kyivCity.Name} {kyivCity.Country.Name}");
        foreach (Studio studio in kyivCity.Studios)
            Console.WriteLine($"{studio.Name}");
    }
}