using AutoMapper;
using Azure;
using Galaxy_Auction_Business.Abstraction;
using Galaxy_Auction_Business.Dtos;
using Galaxy_Auction_Core.MailHelper;
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

public class BidService : IBidService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private ApiResponse _response;
    private readonly IMailService _mailService;

    public BidService(ApplicationDbContext context, IMapper mapper, ApiResponse response, IMailService mailService)
    {
        _context = context;
        _mapper = mapper;
        _response = response;
        _mailService = mailService;
    }

    public async Task<ApiResponse> AutomaticallyCreateBid(CreateBidDto model)
    {
        var isPaid = await CheckIsPaidAuction(model.UserId, model.VehicleId);
        if (!isPaid)
        {
            _response.isSuccess = false;
            _response.ErrorMessages.Add("You must pay the auction fee before placing a bid.");
            return _response;
        }
        var result = await _context.Bids.Where(x => x.VehicleId == model.VehicleId && x.Vehicle.IsActive == true).OrderByDescending(x => x.BidAmount).ToListAsync();
        if (result.Count==0)
        {
            _response.isSuccess = false;
            _response.ErrorMessages.Add("No bids found for this vehicle.");
            return _response;
        }
        var objDto=_mapper.Map<Bid>(model);
        objDto.BidAmount=result[0].BidAmount + (result[0].BidAmount * 10) / 100;
        objDto.BidDate = DateTime.Now;
        _context.Bids.Add(objDto);
        await _context.SaveChangesAsync();
        _response.isSuccess = true;
        _response.Result = objDto;
        return _response;
    }

    public async Task<ApiResponse> CancelBid(int bidId)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse> CreateBid(CreateBidDto model)
    {
        var returnValue = await CheckIsActive(model.VehicleId);
        var checkPaid = await CheckIsPaidAuction(model.UserId, model.VehicleId);

        //if (!checkPaid)
        //{
        //    _response.isSuccess = false;
        //    _response.ErrorMessages.Add("You must pay the auction fee before placing a bid.");
        //    return _response;
        //}


        if (returnValue == null)
        {
            _response.isSuccess = false;
            _response.ErrorMessages.Add("Vehicle is not active or auction has ended.");
            return _response;
        }
        if (returnValue.Price >= model.BidAmount)
        {
            _response.isSuccess = false;
            _response.ErrorMessages.Add("Bid amount must be greater than the current price.");
            return _response;
        }
        if (model != null)
        {
            var topPrice = await _context.Bids.Where(x => x.VehicleId == model.VehicleId).OrderByDescending(x => x.BidAmount).ToListAsync();
            if (topPrice.Count != 0)
            {
                if (topPrice[0].BidAmount >= model.BidAmount)
                {
                    _response.isSuccess = false;
                    _response.ErrorMessages.Add("Bid amount must be greater than the current highest bid.Higher price is :" + topPrice[0].BidAmount);
                    return _response;
                }
            }
            Bid newBid = _mapper.Map<Bid>(model);
            newBid.BidDate = DateTime.Now;
            await _context.Bids.AddAsync(newBid);
            if (await _context.SaveChangesAsync() > 0)
            {
                var userDetail = await _context.Bids.Include(x => x.User).Where(x => x.UserId == model.UserId).FirstOrDefaultAsync();
                _mailService.SendEmail("Your bid has been successfully created.", "Your bis is:" + newBid.BidAmount,newBid.User.UserName);
                _response.isSuccess = true;
                _response.Result = newBid;
                return _response;
            }
        }
        _response.isSuccess = false;
        _response.ErrorMessages.Add("Bid creation failed.");
        return _response;
    }

    public async Task<ApiResponse> GetBidById(int bidId)
    {
        var result = await _context.Bids.Include(x => x.User).Where(x => x.BidId == bidId).FirstOrDefaultAsync();
        if (_response == null)
        {
            _response.isSuccess = false;
            _response.ErrorMessages.Add("Bid is not found.");
            return _response;
        }
        _response.isSuccess = true;
        _response.Result = result;
        return _response;
    }

    public async Task<ApiResponse> GetBidByVehicleId(int vehicleId)
    {
        var obj =await _context.Bids.Where(x => x.VehicleId == vehicleId).ToListAsync();
        if (obj == null)
        {
            _response.isSuccess = false;
            _response.ErrorMessages.Add("No bids found for this vehicle.");
            return _response;
        }
        _response.isSuccess = true;
        _response.Result = obj;
        return _response;
    }

    public async Task<ApiResponse> UpdateBid(int bidId, UpdateBidDto model)
    {
        //Update eden kullanici en son verdigim teklifin uzerine cikmalidir.
        var isPaid = await CheckIsPaidAuction(model.UserId, model.VehicleId);
        if (!isPaid)
        {
            _response.isSuccess = false;
            _response.ErrorMessages.Add("You must pay the auction fee before placing a bid.");
            return _response;
        }
        var result = await _context.Bids.FindAsync(bidId);
        if (result == null)
        {
            _response.isSuccess = false;
            _response.ErrorMessages.Add("Bid is not found.");
            return _response;
        }
        if (result.BidAmount < model.BidAmount && result.UserId == model.UserId)
        {
            var objDTO = _mapper.Map(model, result);
            objDTO.BidDate = DateTime.Now;
            _response.isSuccess = true;
            _response.Result = objDTO;
            await _context.SaveChangesAsync();
            return _response;

        }
        else if (result.BidAmount >= model.BidAmount)
        {
            _response.isSuccess = false;
            _response.ErrorMessages.Add("Bid amount must be greater than the current highest bid.Your older bid amount is:" + result.BidAmount);
            return _response;
        }
        _response.isSuccess = false;
        _response.ErrorMessages.Add("Bid update failed.");
        return _response;


    }

    private async Task<Vehicle> CheckIsActive(int vehicleId)
    {
        var obj = await _context.Vehicles.Where(x => x.VehicleId == vehicleId && x.IsActive == true && x.EndTime >= DateTime.Now).FirstOrDefaultAsync();
        if (obj == null)
        {
            return null;
        }
        else
        {
            return obj;
        }
    }

    private async Task<bool> CheckIsPaidAuction(string userId, int vehicleId)
    {
        var obj = await _context.PaymentHistories.Where(x => x.UserId == userId && x.VehicleId == vehicleId && x.IsActive == true).FirstOrDefaultAsync();
        if (obj == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
