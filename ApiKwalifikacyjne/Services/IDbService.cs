using ApiKwalifikacyjne.Entities;

namespace ApiKwalifikacyjne.Services;

public interface IDbService
{
    Task<IEnumerable<Tag>> Get(string field, string queryOrder, int page);
    Task Add(IEnumerable<Tag> tags);
}