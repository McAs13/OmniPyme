using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OmniPyme.Data;
using OmniPyme.Data.Entities;
using OmniPyme.Web.Core;
using OmniPyme.Web.DTOs;
using OmniPyme.Web.Helpers;

namespace OmniPyme.Web.Services
{
    public interface IPersonsService
    {
        public Task<Response<List<PersonDTO>>> GetListAsync();
    }

    public class PersonsService : IPersonsService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public PersonsService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Response<List<PersonDTO>>> GetListAsync()
        {
            try
            {
                List<Person> persons = await _context.Persons.ToListAsync();

                List<PersonDTO> list = _mapper.Map<List<PersonDTO>>(persons);

                return ResponseHelper<List<PersonDTO>>.MakeResponseSuccess(list);
            }
            catch (Exception ex)
            {
                return ResponseHelper<List<PersonDTO>>.MakeResponseFail(ex);
            }
        }
    }
}
