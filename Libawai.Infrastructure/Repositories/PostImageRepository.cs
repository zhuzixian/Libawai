using Libawai.Core.Entities;
using Libawai.Core.Interfaces;
using Libawai.Infrastructure.Database;
using Libawai.Infrastructure.Database.EntityConfigurations;

namespace Libawai.Infrastructure.Repositories
{
    public class PostImageRepository:IPostImageRepository
    {
        private readonly LibawaiDbContext _context;

        public PostImageRepository(LibawaiDbContext context)
        {
            _context = context;
        }

        public void Add(PostImage postImage)
        {
            _context.PostImages.Add(postImage);
        }


    }
}
