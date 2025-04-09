using OmniPyme.Web.Helpers;
using OmniPyme.Web.Models;
using OmniPyme.Web.Responses;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OmniPyme.Web.Data;
using OmniPyme.Web.Data.Entities;
using OmniPyme.Web.Helpers;
using OmniPyme.Web.Services.Interfaces;


namespace OmniPyme.Web.Services.Interfaces
{
    public interface IProductCategoriesService
    {
        Task<Response<ProductCategoryDTO>> CreateAsync(ProductCategoryDTO model);
        Task<Response<ProductCategoryDTO>> EditAsync(ProductCategoryDTO model);
        Task<Response<ProductCategoryDTO>> DeleteAsync(int id);
        Task<Response<ProductCategoryDTO>> GetOneAsync(int id);
        Task<Response<List<ProductCategoryDTO>>> GetListAsync();
        Task<PaginationResponse<ProductCategoryDTO>> GetPaginationAsync(PaginationDTO model);
    }
}

}
}
public class ProductCategoriesService : CustomQueryableOperations<ProductCategory>, IProductCategoriesService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly ResponseHelper<ProductCategoryDTO> _response;

    public ProductCategoriesService(DataContext context, IMapper mapper)
        : base(context)
    {
        _context = context;
        _mapper = mapper;
        _response = new ResponseHelper<ProductCategoryDTO>();
    }

    public async Task<Response<ProductCategoryDTO>> CreateAsync(ProductCategoryDTO model)
    {
        try
        {
            var entity = _mapper.Map<ProductCategory>(model);
            _context.Add(entity);
            await _context.SaveChangesAsync();

            return _response.SuccessResponse(_mapper.Map<ProductCategoryDTO>(entity));
        }
        catch (Exception ex)
        {
            return _response.ErrorResponse("Error al crear la categoría", ex);
        }
    }

    public async Task<Response<ProductCategoryDTO>> EditAsync(ProductCategoryDTO model)
    {
        try
        {
            var entity = await _context.ProductCategories.FindAsync(model.Id);
            if (entity == null)
                return _response.NotFoundResponse("Categoría no encontrada");

            _mapper.Map(model, entity);
            _context.Update(entity);
            await _context.SaveChangesAsync();

            return _response.SuccessResponse(_mapper.Map<ProductCategoryDTO>(entity));
        }
        catch (Exception ex)
        {
            return _response.ErrorResponse("Error al editar la categoría", ex);
        }
    }

    public async Task<Response<ProductCategoryDTO>> DeleteAsync(int id)
    {
        try
        {
            var entity = await _context.ProductCategories.FindAsync(id);
            if (entity == null)
                return _response.NotFoundResponse("Categoría no encontrada");

            _context.Remove(entity);
            await _context.SaveChangesAsync();

            return _response.SuccessResponse(_mapper.Map<ProductCategoryDTO>(entity));
        }
        catch (Exception ex)
        {
            return _response.ErrorResponse("Error al eliminar la categoría", ex);
        }
    }

    public async Task<Response<ProductCategoryDTO>> GetOneAsync(int id)
    {
        var entity = await _context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id);
        if (entity == null)
            return _response.NotFoundResponse("Categoría no encontrada");

        return _response.SuccessResponse(_mapper.Map<ProductCategoryDTO>(entity));
    }

    public async Task<Response<List<ProductCategoryDTO>>> GetListAsync()
    {
        var list = await _context.ProductCategories
            .OrderBy(x => x.ProductCategoryName)
            .ToListAsync();

        return new Response<List<ProductCategoryDTO>>
        {
            WasSuccess = true,
            Result = _mapper.Map<List<ProductCategoryDTO>>(list)
        };
    }

    public async Task<PaginationResponse<ProductCategoryDTO>> GetPaginationAsync(PaginationDTO model)
    {
        var query = _context.ProductCategories.AsQueryable();

        if (!string.IsNullOrEmpty(model.Filter))
            query = query.Where(x => x.ProductCategoryName.Contains(model.Filter));

        return await GetPaginationAsync<ProductCategory, ProductCategoryDTO>(query, model, _mapper);
    }
}
}