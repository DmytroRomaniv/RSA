using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SupeRSAfe.DAL.Entities;
using SupeRSAfe.DTO.Models;
using AutoMapper;

namespace SupeRSAfe.AutoMapperProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<KeyDTO, Key>().ReverseMap();
            CreateMap<EmailDTO, Email>().ReverseMap();
        }
    }
}
