using Galaxy_Auction_Business.Abstraction;
using Galaxy_Auction_Business.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Galaxy_Auction_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BidController : ControllerBase
{
    private readonly IBidService _bidService;

    public BidController(IBidService bidService)
    {
        _bidService = bidService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateBid([FromBody] CreateBidDto bidDto)
    {
        if(ModelState.IsValid)
        {
            var result = await _bidService.CreateBid(bidDto);
            if (result.isSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result); // Hata mesajlarını döndür
        }
        return BadRequest(ModelState);
    }

    [HttpGet("{bidId}")]
    public async Task<IActionResult> GetBidById(int bidId)
    {
        var result = await _bidService.GetBidById(bidId);
        if (result.isSuccess)
        {
            return Ok(result);
        }
        return NotFound(result); // Hata mesajlarını döndür
    }

    [HttpPut("update/{bidId}")]
    public async Task<IActionResult> UpdateBid(int bidId, [FromBody] UpdateBidDto bidDto)
    {
        if (ModelState.IsValid)
        {
            var result = await _bidService.UpdateBid(bidId, bidDto);
            if (result.isSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result); // Hata mesajlarını döndür
        }
        return BadRequest(ModelState);
    }

    [HttpDelete("cancel/{bidId}")]
    public async Task<IActionResult> CancelBid(int bidId)
    {
        var result = await _bidService.CancelBid(bidId);
        if (result.isSuccess)
        {
            return Ok(result);
        }
        return NotFound(result); // Hata mesajlarını döndür
    }

    [HttpPost("auto-create")]
    public async Task<IActionResult> AutomaticallyCreateBid([FromBody] CreateBidDto bidDto)
    {
        if (ModelState.IsValid)
        {
            var result = await _bidService.AutomaticallyCreateBid(bidDto);
            if (result.isSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result); // Hata mesajlarını döndür
        }
        return BadRequest(ModelState);
    }

    [HttpGet("getbitsbyvehicle/{vehicleId}")]
    public async Task<IActionResult> GetBidByVehicleId(int vehicleId)
    {
        var result = await _bidService.GetBidByVehicleId(vehicleId);
        if (result.isSuccess)
        {
            return Ok(result);
        }
        return NotFound(result); // Hata mesajlarını döndür
    }
} 