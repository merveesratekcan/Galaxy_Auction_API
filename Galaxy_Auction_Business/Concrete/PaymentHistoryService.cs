using AutoMapper;
using Galaxy_Auction_Business.Abstraction;
using Galaxy_Auction_Business.Dtos;
using Galaxy_Auction_Core.Models;
using Galaxy_Auction_Data_Access.Context;
using Galaxy_Auction_Data_Access.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Auction_Business.Concrete;

public class PaymentHistoryService : IPaymentHistoryService
{
    private ApiResponse _apiResponse;
    private readonly ApplicationDbContext _context;
    public IMapper _mapper;
    public PaymentHistoryService(ApiResponse apiResponse,ApplicationDbContext context, IMapper mapper)
    {
        _apiResponse = apiResponse;
        _context = context;
        _mapper = mapper;
    }
    public async Task<ApiResponse> CheckIsStatusForAuction(string userId, int vehicleId)
    {
       var response=await _context.PaymentHistories
            .Where(x => x.UserId == userId && x.VehicleId == vehicleId && x.IsActive == true)
            .FirstOrDefaultAsync();
        if (response != null)
        {
            _apiResponse.isSuccess = true;
            _apiResponse.Result = response;
            return _apiResponse;
        }
        _apiResponse.isSuccess = false;
        _apiResponse.ErrorMessages.Add("Payment history not found or vehicle is not active.");
        return _apiResponse;
    }

    public async Task<ApiResponse> CreatePaymentHistory(CreatePaymentHistoryDto model)
    {
        if (model == null)
        {
          _apiResponse.isSuccess = false;
          _apiResponse.ErrorMessages.Add("Model is null");
           return _apiResponse;
        }
        else
        {
         var objDTO = _mapper.Map<PaymentHistory>(model);
          objDTO.PayDate = DateTime.Now;
            objDTO.IsActive = true;
             _context.PaymentHistories.Add(objDTO);
            if(await _context.SaveChangesAsync() > 0)
            {
                _apiResponse.isSuccess = true;
                _apiResponse.Result = model;
                return _apiResponse;
            }
           
                _apiResponse.isSuccess = false;
                _apiResponse.ErrorMessages.Add("Error while saving to database");
            return _apiResponse;

        }
    }
}
