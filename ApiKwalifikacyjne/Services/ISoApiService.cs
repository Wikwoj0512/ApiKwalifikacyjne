using ApiKwalifikacyjne.Entities;

namespace ApiKwalifikacyjne.Services;

public interface ISoApiService
{
    Task<IEnumerable<Tag>> GetData();
}