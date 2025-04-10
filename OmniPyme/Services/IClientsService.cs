using System.Linq;
using AutoMapper;
using Humanizer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OmniPyme.Data;
using OmniPyme.Web.Core;
using OmniPyme.Web.Core.Pagination;
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
        public Task<Response<PaginationResponse<ClientDTO>>> GetPaginationAsync(PaginationRequest request);

        //public Task<Response<object>> ToggleAsync(ToggleClientStatusDTO dto); //No se usa en esta clase pero se puede implementar en el futuro
    }

    public class ClientsService : CustomQueryableOperations, IClientsService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ClientsService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<ClientDTO>> CreateAsync(ClientDTO dto)
        {
            //try
            //{
            //    Client client = _mapper.Map<Client>(dto);

            //    await _context.AddAsync(client);
            //    await _context.SaveChangesAsync();

            //    return ResponseHelper<ClientDTO>.MakeResponseSuccess(dto, "Cliente creado con éxito");
            //}
            //catch (Exception ex)
            //{
            //    if (ex.InnerException is SqlException sqlEx && sqlEx.Message.Contains("IX_Clients_DNI"))
            //    {
            //        return ResponseHelper<ClientDTO>.MakeResponseFail("El documento ya está registrado en el sistema.");
            //    }

            //    return ResponseHelper<ClientDTO>.MakeResponseFail(ex);
            //}

            return await CreateAsync<Client, ClientDTO>(dto);
        }

        public async Task<Response<object>> DeleteAsync(int id)
        {
            //try
            //{
            //    Response<ClientDTO> response = await GetOneAsync(id);

            //    if (!response.IsSuccess)
            //    {
            //        return ResponseHelper<object>.MakeResponseFail(response.Message);
            //    }

            //    Client client = _mapper.Map<Client>(response.Result);
            //    _context.Clients.Remove(client);
            //    //TODO: Validar si esto se puede hacer o es mas optimo el de arriba
            //    //_context.Clients.Remove(_mapper.Map<Client>(response.Result));

            //    await _context.SaveChangesAsync();

            //    return ResponseHelper<object>.MakeResponseSuccess("Cliente eliminado con éxito");

            //}
            //catch (Exception ex)
            //{
            //    return ResponseHelper<object>.MakeResponseFail(ex);
            //}

            Response<object> response = await DeleteAsync<Client>(id);
            response.Message = !response.IsSuccess ? $"El cliente con id: {id} no existe" : response.Message;
            return response;
        }

        public async Task<Response<ClientDTO>> EditAsync(ClientDTO dto)
        {
            //try
            //{
            //    Response<ClientDTO> responseDto = await GetOneAsync(dto.Id);

            //    if (!responseDto.IsSuccess)
            //    {
            //        return responseDto;
            //    }

            //    Client client = _mapper.Map<Client>(dto);

            //    _context.Entry(client).State = EntityState.Modified;
            //    await _context.SaveChangesAsync();

            //    return ResponseHelper<ClientDTO>.MakeResponseSuccess(dto, "Cliente actualizado con éxito");
            //}
            //catch (Exception ex)
            //{
            //    if (ex.InnerException is SqlException sqlEx && sqlEx.Message.Contains("IX_Clients_DNI"))
            //    {
            //        return ResponseHelper<ClientDTO>.MakeResponseFail("El documento ya está registrado en el sistema.");
            //    }

            //    return ResponseHelper<ClientDTO>.MakeResponseFail(ex);
            //}

            return await EditAsync<Client, ClientDTO>(dto, dto.Id);
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
            //try
            //{
            //    Client? client = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            //    if (client is null)
            //    {
            //        return ResponseHelper<ClientDTO>.MakeResponseFail($"Cliente con id {id} no encontrado");
            //    }

            //    ClientDTO dto = _mapper.Map<ClientDTO>(client);

            //    return ResponseHelper<ClientDTO>.MakeResponseSuccess(dto, "Cliente obtenido con éxito");
            //}
            //catch (Exception ex)
            //{
            //    return ResponseHelper<ClientDTO>.MakeResponseFail(ex);
            //}

            return await GetOneAsync<Client, ClientDTO>(id);
        }

        public async Task<Response<PaginationResponse<ClientDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            //try
            //{
            //    IQueryable<Client> query = _context.Clients.AsNoTracking().AsQueryable();

            //    PagedList<Client> list = await PagedList<Client>.ToPagedListAsync(query, request);

            //    PaginationResponse<ClientDTO> response = new PaginationResponse<ClientDTO>
            //    {
            //        List = _mapper.Map<PagedList<ClientDTO>>(list),
            //        TotalPages = list.TotalPages,
            //        CurrentPage = list.CurrentPage,
            //        RecordsPerPage = list.RecordsPerPage,
            //        TotalRecords = list.TotalRecords
            //    };

            //    return ResponseHelper<PaginationResponse<ClientDTO>>.MakeResponseSuccess(response);
            //}
            //catch (Exception ex)
            //{
            //    return ResponseHelper<PaginationResponse<ClientDTO>>.MakeResponseFail(ex);
            //}

            IQueryable<Client> query = _context.Clients.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.Filter))
            {
                query = query.Where(c => c.DNI.ToLower().Contains(request.Filter.ToLower())
                                        || c.FirstName.ToLower().Contains(request.Filter.ToLower())
                                        || c.LastName.ToLower().Contains(request.Filter.ToLower())
                                        || (c.FirstName + " " + c.LastName).ToLower().Contains(request.Filter.ToLower())
                                        || (c.LastName + " " + c.FirstName).ToLower().Contains(request.Filter.ToLower()));
            }


            return await GetPaginationAsync<Client, ClientDTO>(request, query);
        }

        //public async Task<Response<object>> ToggleAsync(ToggleClientStatusDTO dto)
        //{
        //    try
        //    {
        //        Response<ClientDTO> response = await GetOneAsync(dto.ClientId);

        //        if (!response.IsSuccess)
        //        {
        //            return ResponseHelper<object>.MakeResponseFail($"No existe un cliente con id {dto.ClientId}");
        //        }

        //        Client client = _mapper.Map<Client>(response.Result);
        //        client.Hide = dto.Hide;

        //        //Lineas equivalentes
        //        //_context.Clients.Update(client);
        //        _context.Entry(client).State = EntityState.Modified;

        //        await _context.SaveChangesAsync();

        //        return ResponseHelper<object>.MakeResponseSuccess("Estado del cliente actualizado con éxito");

        //    }
        //    catch (Exception ex)
        //    {
        //        return ResponseHelper<object>.MakeResponseFail(ex);
        //    }
        //}
    }
}
