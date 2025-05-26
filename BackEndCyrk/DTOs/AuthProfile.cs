using AutoMapper;
using BackEndCyrk.Models;

namespace BackEndCyrk.DTOs
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<UserLoginDto, User>();
            CreateMap<UserRegisterDto, User>();
            CreateMap<AuthResultDto, User>();
        }
    }
}