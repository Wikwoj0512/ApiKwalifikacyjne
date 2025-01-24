using System.Collections;
using System.Text.Json;
using ApiKwalifikacyjne.Entities;
using ApiKwalifikacyjne.Models;
using ApiKwalifikacyjne.Services;

namespace ApiKwalifikacyjne.Providers;

public class SoApiService : ISoApiService
{
    private readonly ILogger _logger;


    private HttpClient _httpClient = new();

    public SoApiService(IConfiguration configuration, ILogger<SoApiService> logger)
    {
        var uri = configuration.GetValue<string>("ApiUrl");
        _httpClient.BaseAddress = new Uri(uri);
        _httpClient.DefaultRequestHeaders.UserAgent.Clear();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "StatApi");
        _logger = logger;
    }

    private static string GetUrl(int page)
    {
        return $"tags?order=desc&sort=popular&pagesize=100&site=stackoverflow&page={page}";
    }

    private async Task<ApiResponse> GetPage(int page)
    {
        _logger.LogInformation($"Getting page {page}");
        HttpResponseMessage response = await _httpClient.GetAsync(GetUrl(page));
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error getting page: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStreamAsync();

        var responseContent = await JsonSerializer.DeserializeAsync<ApiResponse>(content);

        if (responseContent == null)
        {
            throw new InvalidDataException("Error getting page");
        }

        return responseContent;
    }

    public async Task<IEnumerable<Tag>> GetData()
    {
        _logger.LogInformation("Starting fetching data from api");

        var hasNext = true;

        List<TagInfo> data = [];

        var totalCount = 0;
        var page = 1;

        while (hasNext && data.Count < 1000)
        {
            var response = await GetPage(page);
            page++;
            hasNext = response.HasMore;
            foreach (var item in response.Items)
            {
                totalCount += item.Count;
                data.Add(item);
            }

            if (response.Backoff > 0)
            {
                await Task.Delay(TimeSpan.FromSeconds((double)response.Backoff));
            }
        }

        return data.Select(x => new Tag
        {
            Count = x.Count,
            Name = x.Name,
            Share = (float)x.Count / totalCount,
            HasSynonyms = x.HasSynonyms,
            IsModeratorOnly = x.IsModeratorOnly,
            IsRequired = x.IsRequired,
        });
    }
}