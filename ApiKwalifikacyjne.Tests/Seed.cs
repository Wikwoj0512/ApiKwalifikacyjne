using System.Collections.Generic;
using ApiKwalifikacyjne.Entities;
using ApiKwalifikacyjne.Helpers;

namespace ApiKwalifikacyjne.Tests;

internal class Seed
{
    public static void InitializeDb(DataContext context)
    {
        context.Tags.AddRange(GetTags());
        context.SaveChanges();
    }

    public static List<Tag> GetTags()
    {
        return new List<Tag>
        {
            new()
            {
                Count = 2, HasSynonyms = false, IsModeratorOnly = false, IsRequired = false, Name = "Tag 1", Share = 0.2
            },
            new()
            {
                Count = 5, HasSynonyms = false, IsModeratorOnly = false, IsRequired = false, Name = "Tag 2", Share = 0.5
            },
            new()
            {
                Count = 3, HasSynonyms = true, IsModeratorOnly = false, IsRequired = false, Name = "Tag 3", Share = 0.3
            }
        };
    }
}