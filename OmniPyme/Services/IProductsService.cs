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
    public interface IProductsService
    {
        Task<Response<ProductDTO>> CreateAsync(ProductDTO model);
        Task<Response<ProductDTO>> EditAsync(ProductDTO model);
        Task<Response<ProductDTO>> DeleteAsync(int id);
        Task<Response<ProductDTO>> GetOneAsync(int id);
        Task<Response<List<ProductDTO>>> GetListAsync();
        Task<PaginationResponse<ProductDTO>> GetPaginationAsync(PaginationDTO model);
    }
}
public class ProductsService : CustomQueryableOperations<Product>, IProductsService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly ResponseHelper<ProductDTO> _response;

    public ProductsService(DataContext context, IMapper mapper)
        : base(context)
    {
        _context = context;
        _mapper = mapper;
        _response = new ResponseHelper<ProductDTO>();
    }

    public async Task<Response<ProductDTO>> CreateAsync(ProductDTO model)
    {
        try
        {
            var entity = _mapper.Map<Product>(model);
            _context.Add(entity);
            await _context.SaveChangesAsync();

            return _response.SuccessResponse(_mapper.Map<ProductDTO>(entity));
        }
        catch (Exception ex)
        {
            return _response.ErrorResponse("Error al crear el producto", ex);
        }
    }

    public async Task<Response<ProductDTO>> EditAsync(ProductDTO model)
    {
        try
        {
            var entity = await _context.Products
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(p => p.Id == model.Id);

            if (entity == null)
                return _response.NotFoundResponse("Producto no encontrado");

            _mapper.Map(model, entity);
            _context.Update(entity);
            await _context.SaveChangesAsync();

            return _response.SuccessResponse(_mapper.Map<ProductDTO>(entity));
        }
        catch (Exception ex)
        {
            return _response.ErrorResponse("Error al editar el producto", ex);
        }
    }

    public async Task<Response<ProductDTO>> DeleteAsync(int id)
    {
        try
        {
            var entity = await _context.Products.FindAsync(id);
            if (entity == null)
                return _response.NotFoundResponse("Producto no encontrado");

            _context.Remove(entity);
            await _context.SaveChangesAsync();

            return _response.SuccessResponse(_mapper.Map<ProductDTO>(entity));
        }
        catch (Exception ex)
        {
            return _response.ErrorResponse("Error al eliminar el producto", ex);
        }
    }

    public async Task<Response<ProductDTO>> GetOneAsync(int id)
    {
        var entity = await _context.Products
            .Include(p => p.ProductCategory)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (entity == null)
            return _response.NotFoundResponse("Producto no encontrado");

        return _response.SuccessResponse(_mapper.Map<ProductDTO>(entity));
    }

    public async Task<Response<List<ProductDTO>>> GetListAsync()
    {
        var list = await _context.Products
            .Include(p => p.ProductCategory)
            .OrderBy(p => p.ProductName)
            .ToListAsync();

        return new Response<List<ProductDTO>>
        {
            WasSuccess = true,
            Result = _mapper.Map<List<ProductDTO>>(list)
        };
    }

    public async Task<PaginationResponse<ProductDTO>> GetPaginationAsync(PaginationDTO model)
    {
        var query = _context.Products
            .Include(p => p.ProductCategory)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(model.Filter))
        {
            query = query.Where(p =>
                p.ProductName.Contains(model.Filter) ||
                p.ProductBarCode.Contains(model.Filter) ||
                p.ProductCategory!.ProductCategoryName.Contains(model.Filter));
        }

        return await GetPaginationAsync<Product, ProductDTO>(query, model, _mapper);
    }
}
}