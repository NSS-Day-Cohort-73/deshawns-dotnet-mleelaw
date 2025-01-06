using System.Net.Http.Headers;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;

var builder = WebApplication.CreateBuilder(args);

List<Dog> dogs = new List<Dog>
{
    new Dog
    {
        Id = 1,
        Name = "Mia",
        CityId = 1,
        WalkerId = 1,
    },
    new Dog
    {
        Id = 2,
        Name = "Carl",
        CityId = 2,
        WalkerId = 1,
    },
};
List<WalkerCity> walkerCities = new List<WalkerCity>
{
    new WalkerCity
    {
        WalkerCityId = 1,
        WalkerId = 1,
        CityId = 1,
    },
    new WalkerCity
    {
        WalkerCityId = 2,
        WalkerId = 1,
        CityId = 2,
    },
    new WalkerCity
    {
        WalkerCityId = 3,
        WalkerId = 2,
        CityId = 3,
    },
    new WalkerCity
    {
        WalkerCityId = 4,
        WalkerId = 2,
        CityId = 4,
    },
    new WalkerCity
    {
        WalkerCityId = 5,
        WalkerId = 3,
        CityId = 1,
    },
    new WalkerCity
    {
        WalkerCityId = 6,
        WalkerId = 3,
        CityId = 2,
    },
    new WalkerCity
    {
        WalkerCityId = 7,
        WalkerId = 3,
        CityId = 4,
    },
    new WalkerCity
    {
        WalkerCityId = 8,
        WalkerId = 4,
        CityId = 2,
    },
    new WalkerCity
    {
        WalkerCityId = 9,
        WalkerId = 4,
        CityId = 3,
    },
    new WalkerCity
    {
        WalkerCityId = 10,
        WalkerId = 4,
        CityId = 4,
    },
};

List<City> cities = new List<City>
{
    new City
    {
        Id = 1,
        Name = "Nashville",
        walkerCities = walkerCities.Where(wc => wc.CityId == 1).ToList(),
    },
    new City
    {
        Id = 2,
        Name = "Portland",
        walkerCities = walkerCities.Where(wc => wc.CityId == 2).ToList(),
    },
    new City
    {
        Id = 3,
        Name = "New York",
        walkerCities = walkerCities.Where(wc => wc.CityId == 3).ToList(),
    },
    new City
    {
        Id = 4,
        Name = "Chicago",
        walkerCities = walkerCities.Where(wc => wc.CityId == 4).ToList(),
    },
};

List<Walker> walkers = new List<Walker>
{
    new Walker
    {
        Id = 1,
        Name = "M'Lee",
        walkerCities = walkerCities.Where(wc => wc.WalkerId == 1).ToList(),
    },
    new Walker
    {
        Id = 2,
        Name = "Eric",
        walkerCities = walkerCities.Where(wc => wc.WalkerId == 2).ToList(),
    },
    new Walker
    {
        Id = 3,
        Name = "Riley",
        walkerCities = walkerCities.Where(wc => wc.WalkerId == 3).ToList(),
    },
    new Walker
    {
        Id = 4,
        Name = "Bee",
        walkerCities = walkerCities.Where(wc => wc.WalkerId == 4).ToList(),
    },
};

// allows for use of navigation props within both walker/city classes.
foreach (var wc in walkerCities)
{
    wc.Walker = walkers.First(w => w.Id == wc.WalkerId);
    wc.City = cities.First(c => c.Id == wc.CityId);
}

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet(
    "/api/hello",
    () =>
    {
        return new { Message = "Welcome to DeShawn's Dog Walking" };
    }
);

// dog endpoints
app.MapGet(
    "/api/dogs",
    () =>
    {
        var dogList = dogs.Select(d => new DogListDTO
        {
            Id = d.Id,
            Name = d.Name,
            WalkerId = d.WalkerId,
            CityId = d.CityId,
        });
        return dogList;
    }
);
app.MapGet(
    "/api/dogs/{id}",
    (int id) =>
    {
        var dogSelected = dogs.FirstOrDefault(d => d.Id == id);

        var city = cities.FirstOrDefault(c => c.Id == dogSelected.CityId);
        string cityName = city.Name;

        var walker = walkers.FirstOrDefault(w => w.Id == dogSelected.WalkerId);
        string walkerName = walker.Name;

        return Results.Ok(
            new DogDetailDTO
            {
                Id = dogSelected.Id,
                Name = dogSelected.Name,
                CityName = cityName,
                WalkerId = dogSelected.WalkerId,
                WalkerName = walkerName,
            }
        );
    }
);
app.MapDelete(
    "/api/dogs/{id}",
    (int id) =>
    {
        Dog deleteDog = dogs.FirstOrDefault(d => d.Id == id);
        dogs.Remove(deleteDog);

        return Results.NoContent();
    }
);
app.MapPost(
    "/api/dogs",
    (Dog newDog) =>
    {
        newDog.Id = dogs.Max(d => d.Id) + 1;
        dogs.Add(newDog);
        return Results.Created($"/api/dogs/{newDog.Id}", newDog);
    }
);

//city endpoints
app.MapGet(
    "/api/cities",
    () =>
    {
        var cityList = cities.Select(c => new CityDTO { Id = c.Id, Name = c.Name });
        return cityList;
    }
);

//walker endpoints
app.MapGet(
    "/api/walkers",
    () =>
    {
        var walkerList = walkers.Select(w => new WalkerDTO { Id = w.Id, Name = w.Name });
        return walkerList;
    }
);

app.MapGet(
    "/api/walkers/{cityid}",
    (int cityid) =>
    {
        return walkers
            .Where(walker => walker.walkerCities.Any(wc => wc.CityId == cityid))
            .Select(walker => new WalkerDTO { Id = walker.Id, Name = walker.Name });
    }
);
app.MapDelete(
    "/api/walkers/{id}",
    (int id) =>
    {
        Walker deleteWalker = walkers.FirstOrDefault(w => w.Id == id);
        walkers.Remove(deleteWalker);

        return Results.NoContent();
    }
);

app.Run();
