using ApiKwalifikacyjne.Entities;

namespace ApiKwalifikacyjne.Services;

public interface IDataService
{
    Task FetchData();
    Task<IEnumerable<Tag>> GetTags(string field, string queryOrder, int? page);
}