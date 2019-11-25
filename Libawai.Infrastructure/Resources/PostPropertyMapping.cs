using System;
using System.Collections.Generic;
using Libawai.Core.Entities;
using Libawai.Infrastructure.Services;

namespace Libawai.Infrastructure.Resources
{
    public class PostPropertyMapping : PropertyMapping<PostResource, Post>
    {
        public PostPropertyMapping() : base(
            new Dictionary<string, List<MappedProperty>>
                (StringComparer.OrdinalIgnoreCase)
                {
                    [nameof(PostResource.Title)] = new List<MappedProperty>
                    {
                        new MappedProperty {Name = nameof(Post.Title), Revert = false}
                    },
                    [nameof(PostResource.Body)] = new List<MappedProperty>
                    {
                        new MappedProperty {Name = nameof(Post.Body), Revert = false}
                    },
                    [nameof(PostResource.Author)] = new List<MappedProperty>
                    {
                        new MappedProperty {Name = nameof(Post.Author), Revert = false}
                    }
                })
        {
        }
    }
}
