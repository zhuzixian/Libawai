using AutoMapper;
using Libawai.Core.Entities;
using Libawai.Infrastructure.Resources;

namespace Libawai.API.Extensions
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, PostResource>()
                .ForMember(dest => dest.UpdateTime,
                    opt => opt.MapFrom(src => src.LastModified));

            CreateMap<PostResource, Post>();
            CreateMap<PostResource, Post>();
            CreateMap<PostUpdateResource, Post>();
            CreateMap<PostAddResource, Post>();

            CreateMap<PostImage, PostImageResource>();
            CreateMap<PostImageResource, PostImage>();
        }
    }
}
