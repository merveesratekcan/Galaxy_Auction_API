using Galaxy_Auction_Business.Abstraction;
using Galaxy_Auction_Business.Dtos;
using Galaxy_Auction_Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Galaxy_Auction_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentHistoryController : ControllerBase
{
    private readonly IPaymentHistoryService _paymentHistoryService;

    public PaymentHistoryController(IPaymentHistoryService paymentHistoryService)
    {
        _paymentHistoryService = paymentHistoryService;
    }

    [HttpPost("CreatePaymentHistory")]
    public async Task<IActionResult> CreatePaymentHistory(CreatePaymentHistoryDto model)
    {
     if(ModelState.IsValid)
        {
            var response = await _paymentHistoryService.CreatePaymentHistory(model);
            if (response.isSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        return BadRequest(ModelState);
    }

    [HttpPost("CheckIsStatusForAuction")]
    public async Task<ActionResult<ApiResponse>> CheckStatusAuction(CheckStatusModel model)
    {
        var response = await _paymentHistoryService.CheckIsStatusForAuction(model.UserId, model.VehicleId);
        return Ok(response);
    }
} 