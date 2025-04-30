using AutoMapper;
using Galaxy_Auction_Business.Abstraction;
using Galaxy_Auction_Business.Dtos;
using Galaxy_Auction_Core.Models;
using Galaxy_Auction_Data_Access.Context;
using Galaxy_Auction_Data_Access.Enums;
using Galaxy_Auction_Data_Access.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Auction_Business.Concrete;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ApiResponse _response;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private string secretKey;

    public UserService(ApplicationDbContext context, IMapper mapper, ApiResponse response, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration _configuration)
    {
        _context = context;
        _mapper = mapper;
        _response = response;
        _userManager = userManager;
        _roleManager = roleManager;
        secretKey = _configuration.GetValue<string>("SecretKey:jwtKey");

    }

    public async Task<ApiResponse> Login(LoginRequestDto model)
    {
        ApplicationUser userFromDb=_context.ApplicationUsers.FirstOrDefault(u=>u.UserName.ToLower() == model.UserName.ToLower());
        if (userFromDb != null)
        {
            bool isValid=await _userManager.CheckPasswordAsync(userFromDb, model.Password);
            if (!isValid)
            {
                _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _response.ErrorMessages.Add("Password is incorrect");
                _response.isSuccess = false;
                return _response;

            }
            var role =await _userManager.GetRolesAsync(userFromDb);
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(secretKey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromDb.Id),
                    new Claim(ClaimTypes.Email, userFromDb.Email),
                    new Claim(ClaimTypes.Role, role.FirstOrDefault()),
                    new Claim("fullName", userFromDb.FullName),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials=new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token= tokenHandler.CreateToken(tokenDescriptor);
            
            LoginResponseModel _model = new()
            {
                Email = userFromDb.Email,
                Token = tokenHandler.WriteToken(token),
            };
            _response.Result = _model;
            _response.isSuccess = true;
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            return _response;
        }
        _response.isSuccess = false;
        _response.ErrorMessages.Add("User not found");
        return _response;
    }

    public async Task<ApiResponse> Register(RegisterRequestDto model)
    {
        var userFromDb= _context.ApplicationUsers.FirstOrDefault(x => x.UserName.ToLower() == model.UserName.ToLower());
        if(userFromDb != null)
        {
            _response.isSuccess = false;
            _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add("User already exists");
            return _response;
        }
        //Automapper kullanarak RegisterRequestDto'dan ApplicationUser'a dönüştür
        //var newUser = _mapper.Map<ApplicationUser>(model);

        ApplicationUser newUser = new()
        {
            FullName = model.FullName,
            UserName = model.UserName,
            NormalizedEmail = model.UserName.ToUpper(),
            Email = model.UserName,
        };

        var result = await _userManager.CreateAsync(newUser, model.Password);

        if (result.Succeeded)
        {
            var isTrue = _roleManager.RoleExistsAsync(UserType.Administrator.ToString()).GetAwaiter().GetResult();
            if (!_roleManager.RoleExistsAsync(UserType.Administrator.ToString()).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(UserType.Administrator.ToString()));
                await _roleManager.CreateAsync(new IdentityRole(UserType.Seller.ToString()));
                await _roleManager.CreateAsync(new IdentityRole(UserType.NormalUser.ToString()));
            
            }
            if(model.UserType.ToString().ToLower() == UserType.Administrator.ToString().ToLower())
            {
                await _userManager.AddToRoleAsync(newUser, UserType.Administrator.ToString());
            }
            if (model.UserType.ToString().ToLower() == UserType.Seller.ToString().ToLower())
            {
                await _userManager.AddToRoleAsync(newUser, UserType.Seller.ToString());
            }
            else if (model.UserType.ToString().ToLower() == UserType.NormalUser.ToString().ToLower())
            {
                await _userManager.AddToRoleAsync(newUser, UserType.NormalUser.ToString());
            }
            _response.StatusCode = System.Net.HttpStatusCode.Created;
            _response.isSuccess = true;
            return _response;
        }
        foreach (var error in result.Errors)
        {
            _response.ErrorMessages.Add(error.ToString());
        }
        return _response;
    }
}
