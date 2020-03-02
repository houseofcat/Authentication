using AutoMapper;
using IdentityServer.Requests;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<IdentityUser, CreateUserRequest>();
            CreateMap<CreateUserRequest, IdentityUser>();
        }
    }
}
