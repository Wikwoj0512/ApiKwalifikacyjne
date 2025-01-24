using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiKwalifikacyjne.Entities;
using ApiKwalifikacyjne.Services;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ApiKwalifikacyjne.Tests.Services;

[TestSubject(typeof(DataService))]
public class DataServiceTest
{
    private readonly Mock<IDbService> _dbServiceMock = new();
    private readonly Mock<ISoApiService> _soApiServiceMock = new();
    private readonly Mock<ILogger> _loggerMock = new();


    private readonly List<Tag> _testTags = new()
    {
        new Tag
        {
            Count = 1, HasSynonyms = false, IsModeratorOnly = false, IsRequired = false, Name = "abc", Share = 0.1
        }
    };

    public DataServiceTest()
    {
        _soApiServiceMock.Setup(x => x.GetData()).ReturnsAsync(_testTags);
        _dbServiceMock.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(_testTags);
    }

    [Fact]
    public async Task FetchData_ShouldSaveTagsToDb()
    {
        var dataService = new DataService(_dbServiceMock.Object, _soApiServiceMock.Object, _loggerMock.Object);

        await dataService.FetchData();

        _dbServiceMock.Verify(x => x.Add(It.IsAny<IEnumerable<Tag>>()), Times.Once);
    }

    [Fact]
    public async Task GetTags_ShouldLoadTagsFromDb()
    {
        var dataService = new DataService(_dbServiceMock.Object, _soApiServiceMock.Object, _loggerMock.Object);

        var result = await dataService.GetTags("", "", null);

        Assert.Equal<IEnumerable<Tag>>(_testTags, result);
    }

    [Fact]
    public async Task GetTags_InvalidField_ShouldThrowException()
    {
        var dataService = new DataService(_dbServiceMock.Object, _soApiServiceMock.Object, _loggerMock.Object);

        await Assert.ThrowsAsync<ArgumentException>(() => dataService.GetTags("abc", "", null));
    }

    [Fact]
    public async Task GetTags_InvalidOrder_ShouldThrowException()
    {
        var dataService = new DataService(_dbServiceMock.Object, _soApiServiceMock.Object, _loggerMock.Object);

        await Assert.ThrowsAsync<ArgumentException>(() => dataService.GetTags("", "abc", null));
    }
}