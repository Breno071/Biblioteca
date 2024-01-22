using AutoMapper;
using Domain.Models.DTO;
using Domain.Models.Entities;

namespace ApplicationCore.Configuration.Mapper.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientDTO>().ReverseMap();
        }
    }
}
