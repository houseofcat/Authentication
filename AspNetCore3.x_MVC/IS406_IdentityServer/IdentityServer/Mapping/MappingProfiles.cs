using AutoMapper;
using IdentityServer.Requests;
using IdentityServer.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Api/v1/AccountController
            CreateMap<IdentityUser, CreateUserRequest>();
            CreateMap<CreateUserRequest, IdentityUser>();

            // AccountController
            CreateMap<IdentityUser, RegisterViewModel>();
            CreateMap<RegisterViewModel, IdentityUser>();
        }
    }
}
