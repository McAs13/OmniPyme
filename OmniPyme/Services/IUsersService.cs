using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OmniPyme.Data;
using OmniPyme.Web.Core.Pagination;
using OmniPyme.Web.Core;
using OmniPyme.Web.Data.Entities;
using OmniPyme.Web.DTOs;
using OmniPyme.Web.Helpers;
using OmniPyme.Web.Services;

using ClaimsUser = System.Security.Claims.ClaimsPrincipal;

namespace OmniPyme.Web.Services
{
    public interface IUsersService
    {
        public Task<IdentityResult> AddUserAsync(Users users, string password);
        public bool CurrentUserIsAuthenticated();
        public Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module);
        public Task<IdentityResult> ConfirmEmailAsync(Users users, string token);
        public Task<string> GenerateEmailConfirmationTokenAsync(Users users);
        public Task<Users> GetUserAsync(string email);
        public Task<SignInResult> LoginAsync(LoginDTO dto);
        public Task LogoutAsync();
        Task<Response<PaginationResponse<UsersDTO>>> GetPaginationAsync(PaginationRequest request);
        Task<Response<UsersDTO>> CreateAsync(UsersDTO dto);
        Task<Users?> GetUserAsync(Guid id);
        Task<Response<UsersDTO>> UpdateUserAsync(UsersDTO dto);
    }

    public class UsersService : CustomQueryableOperations, IUsersService
    {
        private readonly DataContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UsersService(DataContext context,
                            UserManager<Users> userManager,
                            SignInManager<Users> signInManager,
                            IHttpContextAccessor httpContextAccessor,
                            IMapper mapper)
            : base(context, mapper)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<IdentityResult> AddUserAsync(Users users, string password)
        {
            return await _userManager.CreateAsync( users, password);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(Users users, string token)
        {
            return await _userManager.ConfirmEmailAsync(users, token);
        }

        public async Task<Response<UsersDTO>> CreateAsync(UsersDTO dto)
        {
            try
            {
                Users users = _mapper.Map<Users>(dto);
                users.Id = Guid.NewGuid().ToString();

                IdentityResult result = await AddUserAsync(users, dto.Document);
                string token = await GenerateEmailConfirmationTokenAsync(users);
                await ConfirmEmailAsync(users, token);

                return ResponseHelper<UsersDTO>.MakeResponseSuccess(_mapper.Map<UsersDTO>(users), "Usuario creado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<UsersDTO>.MakeResponseFail(ex);
            }
        }

        public bool CurrentUserIsAuthenticated()
        {
            ClaimsUser? user = _httpContextAccessor.HttpContext?.User;
            return user?.Identity != null && user.Identity.IsAuthenticated;
        }

        public async Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module)
        {
            ClaimsUser? claimsUser = _httpContextAccessor.HttpContext?.User;

            // Valida si hay sesión
            if (claimsUser is null)
            {
                return false;
            }

            string? userName = claimsUser.Identity!.Name;

            Users? users = await GetUserAsync(userName);

            if (users is null)
            {
                return false;
            }

            if (users.PrivateURole.Name == Env.SUPER_ADMIN_ROLE_NAME)
            {
                return true;
            }

            return await _context.Permissions.Include(p => p.RolePermissions)
                                             .AnyAsync(p => (p.Module == module && p.Name == permission)
                                                            && p.RolePermissions.Any(rp => rp.Roleid == users.PrivateURoleId));
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(Users users)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(users);
        }

        public async Task<Response<PaginationResponse<UsersDTO>>> GetPaginationAsync(PaginationRequest request)
        {

            IQueryable<Users> query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(request.Filter))
            {
                query = query.Where(b => b.FirstName.ToLower().Contains(request.Filter.ToLower())
                                      || b.LastName.ToLower().Contains(request.Filter.ToLower())
                                      || b.Document.Contains(request.Filter)
                                      || b.Email.ToLower().Contains(request.Filter.ToLower())
                                      || b.PhoneNumber.Contains(request.Filter));
            }

            return await GetPaginationAsync<Users, UsersDTO>(request, query);
        }

        public async Task<Users> GetUserAsync(string email)
        {
            Users? users = await _context.Users.Include(u => u.PrivateURole)
                                             .FirstOrDefaultAsync(u => u.Email == email);

            return users;
        }

        public async Task<Users?> GetUserAsync(Guid id)
        {
            return await _context.Users.Include(u => u.PrivateURole)
                                       .FirstOrDefaultAsync(u => u.Id == id.ToString());
        }

        public async Task<SignInResult> LoginAsync(LoginDTO dto)
        {
            return await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<Response<UsersDTO>> UpdateUserAsync(UsersDTO dto)
        {
            try
            {
                //User user = _mapper.Map<User>(dto);

                Guid id = Guid.Parse(dto.Id!);
                Users users = await GetUserAsync(id);
                users.PhoneNumber = dto.PhoneNumber;
                users.Document = dto.Document;
                users.FirstName = dto.FirstName;
                users.LastName = dto.LastName;
                users.PrivateURoleId = dto.PrivateURoleId;

                _context.Users.Update(users);

                await _context.SaveChangesAsync();

                return ResponseHelper<UsersDTO>.MakeResponseSuccess(dto, "Usuario actualizado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<UsersDTO>.MakeResponseFail(ex);
            }
        }
    }
}