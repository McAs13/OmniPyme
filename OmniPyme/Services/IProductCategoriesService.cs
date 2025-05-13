using OmniPyme.Web.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OmniPyme.Web.Data.Entities;
using OmniPyme.Web.Core;
using OmniPyme.Web.Core.Pagination;
using OmniPyme.Data;
using OmniPyme.Web.DTOs;

namespace OmniPyme.Web.Services
{
    public interface IProductCategoriesService
    {
        Task<Response<ProductCategoryDTO>> CreateAsync(ProductCategoryDTO dto);
        Task<Response<ProductCategoryDTO>> EditAsync(ProductCategoryDTO dto);
        Task<Response<object>> DeleteAsync(int id);
        Task<Response<ProductCategoryDTO>> GetOneAsync(int id);
        Task<Response<PaginationResponse<ProductCategoryDTO>>> GetPaginationAsync(PaginationRequest request);
    }
    public class ProductCategoriesService : CustomQueryableOperations, IProductCategoriesService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProductCategoriesService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<ProductCategoryDTO>> CreateAsync(ProductCategoryDTO dto)
        {
            return await CreateAsync<ProductCategory, ProductCategoryDTO>(dto);
        }

        public async Task<Response<ProductCategoryDTO>> EditAsync(ProductCategoryDTO dto)
        {
            return await EditAsync<ProductCategory, ProductCategoryDTO>(dto, dto.Id);
        }

        public async Task<Response<object>> DeleteAsync(int id)
        {
            Response<object> response = await DeleteAsync<ProductCategory>(id);
            response.Message = !response.IsSuccess ? $"La categoría con id: {id} no existe" : response.Message;
            return response;
        }

        public async Task<Response<ProductCategoryDTO>> GetOneAsync(int id)
        {
            try
            {
                ProductCategory? entity = await _context.ProductCategories
                .FirstOrDefaultAsync(x => x.Id == id);
                if (entity is null)
                {
                    return ResponseHelper<ProductCategoryDTO>.MakeResponseFail($"No existe la categoría con id {id}");
                }

                ProductCategoryDTO dTO = _mapper.Map<ProductCategoryDTO>(entity);
                return ResponseHelper<ProductCategoryDTO>.MakeResponseSuccess(dTO, "Registro encontrado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<ProductCategoryDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PaginationResponse<ProductCategoryDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            IQueryable<ProductCategory> query = _context.ProductCategories.AsQueryable();

            if (!string.IsNullOrEmpty(request.Filter))
                query = query.Where(x => x.ProductCategoryName.Contains(request.Filter));

            return await GetPaginationAsync<ProductCategory, ProductCategoryDTO>(request, query);
        }
    }
}