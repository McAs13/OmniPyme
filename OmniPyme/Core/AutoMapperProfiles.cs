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
            CreateMap<Invoice, InvoiceDTO>().ReverseMap();
            CreateMap<Sale, SaleDTO>().ReverseMap();
            CreateMap<SaleDetail, SaleDetailDTO>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<PrivateURole, PrivateURoleDTO>().ReverseMap();
            CreateMap<Permission, PermissionDTO>().ReverseMap();
            CreateMap<Users, UsersDTO>();
            CreateMap<UsersDTO, Users>().ForMember(users=> users.UserName, Config=>Config
                                        .MapFrom(dto=>dto.Email));

        }
    }
}
