using Galaxy_Auction_Business.Dtos;
using Galaxy_Auction_Core.Comman;
using Galaxy_Auction_Core.Models;
using Galaxy_Auction_Data_Access.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;

namespace Galaxy_Auction.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly ApiResponse _apiResponse;
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    private StripeSettings _stripeSettings;
    public PaymentController(IConfiguration configuration,IOptions<StripeSettings> options, ApplicationDbContext context, ApiResponse apiResponse)
    {
        _apiResponse = apiResponse;
        _configuration = configuration;
        _context = context;
        _stripeSettings = options.Value;
    }

    [HttpPost("create-payment")]
    public async Task<ActionResult<ApiResponse>> MakePayment(string userId, int vehicleId)
    {
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        var amountToBePaid = await _context.Vehicles.FirstOrDefaultAsync(x => x.VehicleId == vehicleId);

        var options = new PaymentIntentCreateOptions
        {
            Amount = (int)(amountToBePaid.AuctionPrice * 100),
            Currency = "usd",
            PaymentMethodTypes = new List<string>
            {
                "card"
            }
        };

        var service = new PaymentIntentService();
        var response = service.Create(options);

        CreatePaymentHistoryDto model= new()
        {
            UserId = userId,
            ClientSecret = response.ClientSecret,
            StripePaymentIntentId = response.Id,
            VehicleId = vehicleId
        };

        _apiResponse.Result = model;
        _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
        _apiResponse.isSuccess = true;
        return Ok(_apiResponse);

    }
}
