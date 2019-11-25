﻿using System.Threading.Tasks;
using Libawai.Core.Entities;

namespace Libawai.Core.Interfaces
{
    public interface IPostRepository
    {
        Task<PaginatedList<Post>> GetAllPostsAsync(PostParameters postParameters);
        Task<Post> GetPostByIdAsync(int id);
        void AddPost(Post post);
        void Delete(Post post);
        void Update(Post post);
    }
}
