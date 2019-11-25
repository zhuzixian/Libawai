using System.Linq;
using System.Threading.Tasks;
using Libawai.Core.Entities;
using Libawai.Core.Interfaces;
using Libawai.Infrastructure.Database;
using Libawai.Infrastructure.Extensions;
using Libawai.Infrastructure.Resources;
using Libawai.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Libawai.Infrastructure.Repositories
{
    public class PostRepository:IPostRepository
    {
        private readonly LibawaiDbContext _context;
        private readonly IPropertyMappingContainer _propertyMappingContainer;

        public PostRepository(LibawaiDbContext context, 
            IPropertyMappingContainer propertyMappingContainer)
        {
            _context = context;
            _propertyMappingContainer = propertyMappingContainer;
        }

        public async Task<PaginatedList<Post>> GetAllPostsAsync(PostParameters postParameters)
        {
            return await Task.Run(() =>
            {
                var query = _context.Posts.ApplySort(
                    postParameters.OrderBy,
                    _propertyMappingContainer.Resolve<PostResource, Post>())
                    .AsEnumerable();

                if (!string.IsNullOrEmpty(postParameters.Title))
                {
                    var title = postParameters.Title.ToLowerInvariant();
                    query = query.Where(x => x.Title.ToLowerInvariant() == title);
                }

                var posts = query.ToList();
                var count = posts.Count();
                var data = posts
                    .Skip(postParameters.PageIndex * postParameters.PageSize)
                    .Take(postParameters.PageSize);

                return new PaginatedList<Post>(
                    postParameters.PageIndex,
                    postParameters.PageSize,
                    count, data);
            });
        }


        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public void AddPost(Post post)
        {
            _context.Posts.Add(post);
        }

        public void Delete(Post post)
        {
            _context.Posts.Remove(post);
        }

        public void Update(Post post)
        {
            _context.Entry(post).State = EntityState.Modified;
        }
    }
}
