using AutoMapper;
using Domain.Models.DTO;
using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Configuration.Mapper.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile() 
        {
            CreateMap<Book, BookDTO>().ReverseMap();
        }
    }
}
