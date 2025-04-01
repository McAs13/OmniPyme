using AutoMapper;
using OmniPyme.Web.Data.Entities;
using OmniPyme.Web.DTOs;

namespace OmniPyme.Web.Core
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Client, ClientDTO>().ReverseMap();
        }
    }
}
