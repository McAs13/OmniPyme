using AutoMapper;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using OmniPyme.Data;
using OmniPyme.Web.Core;
using OmniPyme.Web.Data.Entities;
using OmniPyme.Web.DTOs;
using OmniPyme.Web.Helpers;

namespace OmniPyme.Web.Services
{
    public interface IClientsService
    {
        public Task<Response<ClientDTO>> CreateAsync(ClientDTO dto);
        public Task<Response<object>> DeleteAsync(int id);
        public Task<Response<ClientDTO>> EditAsync(ClientDTO dto);
        public Task<Response<List<ClientDTO>>> GetListAsync();
        public Task<Response<ClientDTO>> GetOneAsync(int id);
    }

    public class ClientsService : IClientsService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ClientsService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<ClientDTO>> CreateAsync(ClientDTO dto)
        {
            try
            {
                Client client = _mapper.Map<Client>(dto);

                await _context.AddAsync(client);
                await _context.SaveChangesAsync();

                return ResponseHelper<ClientDTO>.MakeResponseSuccess(dto, "Cliente creado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<ClientDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<object>> DeleteAsync(int id)
        {
            try
            {
                Response<ClientDTO> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return ResponseHelper<object>.MakeResponseFail(response.Message);
                }

                Client client = _mapper.Map<Client>(response.Result);
                _context.Clients.Remove(client);
                //TODO: Validar si esto se puede hacer o es mas optimo el de arriba
                //_context.Clients.Remove(_mapper.Map<Client>(response.Result));

                await _context.SaveChangesAsync();

                return ResponseHelper<object>.MakeResponseSuccess("Cliente eliminado con éxito");

            }
            catch (Exception ex)
            {
                return ResponseHelper<object>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<ClientDTO>> EditAsync(ClientDTO dto)
        {
            try
            {
                Response<ClientDTO> responseDto = await GetOneAsync(dto.IdClient);

                if (!responseDto.IsSuccess)
                {
                    return responseDto;
                }

                Client client = _mapper.Map<Client>(dto);

                _context.Entry(client).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return ResponseHelper<ClientDTO>.MakeResponseSuccess(dto, "Cliente actualizado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<ClientDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<List<ClientDTO>>> GetListAsync()
        {
            try
            {
                List<Client> clients = await _context.Clients.ToListAsync();

                List<ClientDTO> list = _mapper.Map<List<ClientDTO>>(clients);

                return ResponseHelper<List<ClientDTO>>.MakeResponseSuccess(list);
            }
            catch (Exception ex)
            {
                return ResponseHelper<List<ClientDTO>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<ClientDTO>> GetOneAsync(int id)
        {
            try
            {
                Client? client = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.IdClient == id);

                if (client is null)
                {
                    return ResponseHelper<ClientDTO>.MakeResponseFail($"Cliente con id {id} no encontrado");
                }

                ClientDTO dto = _mapper.Map<ClientDTO>(client);

                return ResponseHelper<ClientDTO>.MakeResponseSuccess(dto, "Cliente obtenido con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<ClientDTO>.MakeResponseFail(ex);
            }
        }
    }
}
