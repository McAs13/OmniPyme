using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OmniPyme.Data;
using OmniPyme.Web.Core;
using OmniPyme.Web.Data.Entities;
using OmniPyme.Web.DTOs;
using OmniPyme.Web.Helpers;

namespace OmniPyme.Web.Services
{
    public interface IRolesService
    {
        Task<Response<RolDTO>> CreateAsync(RolDTO dto);
        Task<Response<object>> DeleteAsync(int id);
        Task<Response<RolDTO>> EditAsync(RolDTO dto);
        Task<Response<List<RolDTO>>> GetListAsync();
        Task<Response<RolDTO>> GetOneAsync(int id);
    }

    public class RolesService : IRolesService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RolesService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<RolDTO>> CreateAsync(RolDTO dto)
        {
            try
            {
                Role role = _mapper.Map<Role>(dto);
                await _context.AddAsync(role);
                await _context.SaveChangesAsync();
                return ResponseHelper<RolDTO>.MakeResponseSuccess(dto, "Rol creado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<RolDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<object>> DeleteAsync(int id)
        {
            try
            {
                Response<RolDTO> response = await GetOneAsync(id);

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

        public async Task<Response<RolDTO>> EditAsync(RolDTO dto)
        {
            try
            {
                Response<RolDTO> response = await GetOneAsync(dto.IdRol);

                if (!response.IsSuccess)
                {
                    return response;
                }

                Role role = _mapper.Map<Role>(dto);
                _context.Entry(role).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return ResponseHelper<RolDTO>.MakeResponseSuccess(dto, "Rol actualizado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<RolDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<List<RolDTO>>> GetListAsync()
        {
            try
            {
                List<Role> roles = await _context.Roles.ToListAsync();
                List<RolDTO> list = _mapper.Map<List<RolDTO>>(roles);
                return ResponseHelper<List<RolDTO>>.MakeResponseSuccess(list);
            }
            catch (Exception ex)
            {
                return ResponseHelper<List<RolDTO>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<RolDTO>> GetOneAsync(int id)
        {
            try
            {
                Role? role = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.IdRol == id);

                if (role is null)
                {
                    return ResponseHelper<RolDTO>.MakeResponseFail($"Rol con id {id} no encontrado");
                }

                RolDTO dto = _mapper.Map<RolDTO>(role);
                return ResponseHelper<RolDTO>.MakeResponseSuccess(dto, "Rol obtenido con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<RolDTO>.MakeResponseFail(ex);
            }
        }
    }
}