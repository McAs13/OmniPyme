using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OmniPyme.Data;
using OmniPyme.Web.Core;
using OmniPyme.Web.Core.Pagination;
using OmniPyme.Web.Data.Entities;
using OmniPyme.Web.DTOs;
using OmniPyme.Web.Helpers;

namespace OmniPyme.Web.Services
{
    public interface IRolesService
    {
        Task<Response<RoleDTO>> CreateAsync(RoleDTO dto);
        Task<Response<object>> DeleteAsync(int id);
        Task<Response<RoleDTO>> EditAsync(RoleDTO dto);
        Task<Response<List<RoleDTO>>> GetListAsync();

        Task<Response<RoleDTO>> GetOneAsync(int id);
        public Task<Response<PaginationResponse<RoleDTO>>> GetPaginationAsync(PaginationRequest request);
    }

    public class RolesService : CustomQueryableOperations, IRolesService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RolesService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<RoleDTO>> CreateAsync(RoleDTO dto)
        {
            try
            {
                Role role = _mapper.Map<Role>(dto);
                await _context.AddAsync(role);
                await _context.SaveChangesAsync();
                return ResponseHelper<RoleDTO>.MakeResponseSuccess(dto, "Rol creado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<RoleDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<object>> DeleteAsync(int id)
        {
            try
            {
                Response<RoleDTO> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return ResponseHelper<object>.MakeResponseFail(response.Message);
                }

                Role role = _mapper.Map<Role>(response.Result);
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();

                return ResponseHelper<object>.MakeResponseSuccess("Rol eliminado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<object>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<RoleDTO>> EditAsync(RoleDTO dto)
        {
            try
            {
                Response<RoleDTO> response = await GetOneAsync(dto.Id);

                if (!response.IsSuccess)
                {
                    return response;
                }

                Role role = _mapper.Map<Role>(dto);
                _context.Entry(role).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return ResponseHelper<RoleDTO>.MakeResponseSuccess(dto, "Rol actualizado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<RoleDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<List<RoleDTO>>> GetListAsync()
        {
            try
            {
                List<Role> roles = await _context.Roles.ToListAsync();
                List<RoleDTO> list = _mapper.Map<List<RoleDTO>>(roles);
                return ResponseHelper<List<RoleDTO>>.MakeResponseSuccess(list);
            }
            catch (Exception ex)
            {
                return ResponseHelper<List<RoleDTO>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<RoleDTO>> GetOneAsync(int id)
        {
            try
            {
                Role? role = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);

                if (role is null)
                {
                    return ResponseHelper<RoleDTO>.MakeResponseFail($"Rol con id {id} no encontrado");
                }

                RoleDTO dto = _mapper.Map<RoleDTO>(role);
                return ResponseHelper<RoleDTO>.MakeResponseSuccess(dto, "Rol obtenido con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<RoleDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PaginationResponse<RoleDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            IQueryable<Role> query = _context.Roles.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.Filter))
            {
                query = query.Where(c =>
                    c.Id.ToString().ToLower().Contains(request.Filter.ToLower()) ||
                    c.RolName.ToLower().Contains(request.Filter.ToLower()));
            }

            query = query.OrderBy(c => c.Id);

            return await GetPaginationAsync<Role, RoleDTO>(request, query);
        }


    }
}