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
    public interface IInvoicesService
    {
        public Task<Response<InvoiceDTO>> CreateAsync(InvoiceDTO dto);
        public Task<Response<object>> DeleteAsync(int id);
        public Task<Response<InvoiceDTO>> EditAsync(InvoiceDTO dto);
        public Task<Response<InvoiceDTO>> GetOneAsync(int id);
        public Task<Response<PaginationResponse<InvoiceDTO>>> GetPaginationAsync(PaginationRequest request);
    }

    public class InvoicesService : CustomQueryableOperations, IInvoicesService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public InvoicesService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<InvoiceDTO>> CreateAsync(InvoiceDTO dto)
        {
            return await CreateAsync<Invoice, InvoiceDTO>(dto);
        }

        public async Task<Response<object>> DeleteAsync(int id)
        {
            Response<object> response = await DeleteAsync<Invoice>(id);
            response.Message = !response.IsSuccess ? $"La factura con id: {id} no existe" : response.Message;
            return response;
        }

        public async Task<Response<InvoiceDTO>> EditAsync(InvoiceDTO dto)
        {
            return await EditAsync<Invoice, InvoiceDTO>(dto, dto.Id);
        }

        public async Task<Response<InvoiceDTO>> GetOneAsync(int id)
        {
            return await GetOneAsync<Invoice, InvoiceDTO>(id);
        }

        public async Task<Response<PaginationResponse<InvoiceDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            return await GetPaginationAsync<Invoice, InvoiceDTO>(request);
        }
    }
}
