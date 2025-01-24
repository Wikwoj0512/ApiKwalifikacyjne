using System.Linq.Expressions;
using ApiKwalifikacyjne.Entities;
using ApiKwalifikacyjne.Helpers;
using ApiKwalifikacyjne.Services;
using Microsoft.EntityFrameworkCore;

namespace ApiKwalifikacyjne;

public class DbService : IDbService
{
    private DataContext _context;
    private readonly ILogger _logger;
    private const int pageSize = 10;

    public DbService(ILogger<DbService> logger, DataContext dataContext)
    {
        _context = dataContext;
        _logger = logger;
    }

    private static Func<Tag, object> GetFieldDescriptor(string field)
    {
        return field switch
        {
            "name" => t => t.Name,
            "share" => t => t.Share,
            _ => throw new ArgumentException($"Field {field} is not supported")
        };
    }

    public async Task<IEnumerable<Tag>> Get(string field, string queryOrder, int page)
    {
        _logger.LogInformation($"Getting tags with params field: {field}, order: {queryOrder}, page: {page}");
        if (queryOrder == "desc")
        {
            return _context.Tags.OrderByDescending(GetFieldDescriptor(field)).Skip(pageSize * (page - 1)).Take(pageSize)
                .ToList();
        }

        return _context.Tags.OrderBy(GetFieldDescriptor(field)).Skip(pageSize * (page - 1)).Take(pageSize).ToList();
    }


    public async Task Add(IEnumerable<Tag> tags)
    {
        _logger.LogInformation("Adding tags to database");
        var enumerable = tags as Tag[] ?? tags.ToArray();
        foreach (var tag in enumerable)
        {
            if (await _context.Tags.AnyAsync(t => t.Name == tag.Name))
            {
                await _context.Tags.AddAsync(tag);
                _context.Entry(tag).State = EntityState.Modified;
            }
            else
            {
                await _context.Tags.AddAsync(tag);
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Added tags to database");
    }
}