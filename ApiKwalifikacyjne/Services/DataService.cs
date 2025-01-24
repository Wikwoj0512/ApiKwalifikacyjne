using ApiKwalifikacyjne.Entities;
using ApiKwalifikacyjne.Providers;
using Microsoft.Extensions.Primitives;

namespace ApiKwalifikacyjne.Services;

public class DataService(IDbService dbService, ISoApiService apiService, ILogger logger) : IDataService
{
    public async Task FetchData()
    {
        logger.LogInformation("Getting data");
        var data = await apiService.GetData();
        logger.LogInformation("Data retrieved, saving");
        await dbService.Add(data);
        logger.LogInformation("Data saved");
    }


    private readonly List<string> _validFields = new() { "name", "share" };
    private readonly List<string> _validOrder = new() { "asc", "desc" };

    private void ValidateInput(string field, string order, int page)
    {
        if (!_validFields.Contains(field))
        {
            throw new ArgumentException($"Field {field} is not valid");
        }

        if (!_validOrder.Contains(order))
        {
            throw new ArgumentException($"Order{order} is not valid");
        }

        if (page < 1)
        {
            throw new ArgumentException($"Page {page} is not in valid range");
        }
    }

    public async Task<IEnumerable<Tag>> GetTags(string field, string queryOrder, int? page)
    {
        if (string.IsNullOrWhiteSpace(field))
        {
            field = "name";
        }

        if (string.IsNullOrWhiteSpace(queryOrder))
        {
            queryOrder = "asc";
        }

        var pageNumber = 1;
        if (page.HasValue && page.Value > 0)
        {
            pageNumber = page.Value;
        }


        ValidateInput(field, queryOrder, pageNumber);

        return await dbService.Get(field, queryOrder, pageNumber);
    }
}