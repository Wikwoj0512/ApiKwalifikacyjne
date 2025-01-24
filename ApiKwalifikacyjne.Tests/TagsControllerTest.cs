using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ApiKwalifikacyjne.Entities;
using ApiKwalifikacyjne.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ApiKwalifikacyjne.Tests;

public class TagsControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public TagsControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetTags_ReturnsTags()
    {
        var scope = _factory.Server.Services.CreateScope();

        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<DataContext>();

        await db.Database.EnsureCreatedAsync();
        Seed.InitializeDb(db);

        try
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/tags");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<List<Tag>>();

            Assert.Equivalent(result, Seed.GetTags());
        }
        finally
        {
            await db.Database.EnsureDeletedAsync();
        }
    }

    [Fact]
    public async Task GetTags_ParmsSortTags()
    {
        var scope = _factory.Server.Services.CreateScope();

        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<DataContext>();

        await db.Database.EnsureCreatedAsync();
        Seed.InitializeDb(db);

        try
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/tags?field=share");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<List<Tag>>();

            Assert.Equivalent(result, Seed.GetTags().OrderBy(x => x.Share));
        }
        finally
        {
            await db.Database.EnsureDeletedAsync();
        }
    }

    [Fact]
    public async Task GetTags_ParmFilterOrder()
    {
        var scope = _factory.Server.Services.CreateScope();

        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<DataContext>();

        await db.Database.EnsureCreatedAsync();
        Seed.InitializeDb(db);

        try
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/tags?order=desc");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<List<Tag>>();

            Assert.Equivalent(result, Seed.GetTags().OrderByDescending(x => x.Name));
        }
        finally
        {
            await db.Database.EnsureDeletedAsync();
        }
    }
}