using AutoMapper;
using Blog.Cms.Api.Models;
using Blog.Domain.Entities;

namespace Blog.Cms.Api.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PostRequest, Post>().ReverseMap();
        CreateMap<CreateUserRequest, User>().ReverseMap();
    }
}