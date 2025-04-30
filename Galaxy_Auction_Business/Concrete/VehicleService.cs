using AutoMapper;
using Galaxy_Auction_Business.Abstraction;
using Galaxy_Auction_Business.Dtos;
using Galaxy_Auction_Core.Models;
using Galaxy_Auction_Data_Access.Context;
using Galaxy_Auction_Data_Access.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Auction_Business.Concrete;

public class VehicleService : IVehicleService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private ApiResponse _response;

    public VehicleService(ApplicationDbContext context, IMapper mapper, ApiResponse response)
    {
        _context = context;
        _mapper = mapper;
        _response = response;
    }

    public async Task<ApiResponse> ChangeVehicleStatus(int vehicleId)
    {
        var result = await _context.Vehicles.FindAsync(vehicleId);
        if (result == null)
        {
          _response.isSuccess = false;
            return _response;
        }
        result.IsActive = false;
        _response.isSuccess=true;
        await _context.SaveChangesAsync();
        return _response;


    }

    public async Task<ApiResponse> CreateVehicle(CreateVehicleDto model)
    {
        if(model != null)
        {
            var objDto = _mapper.Map<Vehicle>(model);
            if (objDto != null)
            {
                 _context.Vehicles.Add(objDto);
                if (await _context.SaveChangesAsync() > 0)
                {
                    _response.isSuccess = true;
                    _response.Result = objDto;
                    _response.StatusCode = System.Net.HttpStatusCode.Created;
                    return _response;
                }
            }
        }
        _response.isSuccess = false;
        _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        _response.ErrorMessages.Add("Vehicle creation failed");
        return _response;
    }

    public async Task<ApiResponse> DeleteVehicle(int vehicleId)
    {
        var result= await _context.Vehicles.FindAsync(vehicleId);
        if (result != null)
        {
            _context.Vehicles.Remove(result);
            if (await _context.SaveChangesAsync() > 0)
            {
                _response.isSuccess = true;
                _response.StatusCode = System.Net.HttpStatusCode.OK;
                return _response;
         
            }
        }
        _response.isSuccess = false;
        _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
        _response.ErrorMessages.Add("Vehicle deletion failed");
        return _response;

    }

    public async Task<ApiResponse> GetVehicle()
    {
        var vehicle=await _context.Vehicles.Include(x => x.Seller).ToListAsync();
        if (vehicle != null)
        {
            _response.isSuccess = true;
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.Result = vehicle;
            return _response;
        }
        _response.isSuccess = false;
        _response.StatusCode = System.Net.HttpStatusCode.NotFound;
        _response.ErrorMessages.Add("No vehicles found");
        return _response;
    }

    public async Task<ApiResponse> GetVehicleById(int vehicleId)
    {
        var result=await _context.Vehicles.Include(x=>x.Seller).FirstOrDefaultAsync(x=>x.VehicleId == vehicleId);
        if(result != null)
        {
            _response.isSuccess = true;
            _response.StatusCode = System.Net.HttpStatusCode.OK;
            _response.Result = result;
            return _response;
        }
        _response.isSuccess=false;
        _response.StatusCode = System.Net.HttpStatusCode.NotFound;
        _response.ErrorMessages.Add("Vehicle not found");
        return _response;

    }

    public async Task<ApiResponse> UpdateVehicleResponse(int vehicleId, UpdateVehicleDto model)
    {
        var result=await _context.Vehicles.FindAsync(vehicleId);
        if(result!=null)
        {
            Vehicle objDto=_mapper.Map(model, result);
            if(await _context.SaveChangesAsync() > 0)
            {
                _response.isSuccess = true;
               _response.Result = objDto;
                return _response;
            }
        
        }
        _response.isSuccess = false;
        return _response;
    }
}
