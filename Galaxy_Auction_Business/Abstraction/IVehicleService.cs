using Galaxy_Auction_Business.Dtos;
using Galaxy_Auction_Core.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Auction_Business.Abstraction;

public interface IVehicleService
{
    Task<ApiResponse> CreateVehicle(CreateVehicleDto model);
    Task<ApiResponse> GetVehicle();
    Task<ApiResponse> UpdateVehicleResponse(int vehicleId,UpdateVehicleDto model);
    Task<ApiResponse> DeleteVehicle(int vehicleId);
    Task<ApiResponse> GetVehicleById(int vehicleId);
    Task<ApiResponse> ChangeVehicleStatus(int vehicleId);
}
