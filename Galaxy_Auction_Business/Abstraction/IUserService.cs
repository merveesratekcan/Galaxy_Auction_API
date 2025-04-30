using Galaxy_Auction_Business.Dtos;
using Galaxy_Auction_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Auction_Business.Abstraction;

public interface IUserService
{
    Task<ApiResponse> Register(RegisterRequestDto model);
    Task<ApiResponse> Login(LoginRequestDto model);
}
