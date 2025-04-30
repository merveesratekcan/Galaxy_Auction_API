using Galaxy_Auction_Business.Abstraction;
using Galaxy_Auction_Business.Dtos;
using Galaxy_Auction_Data_Access.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Galaxy_Auction_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;
    public readonly IWebHostEnvironment _webHostEnvironment;

    public VehicleController(IVehicleService vehicleService, IWebHostEnvironment webHostEnvironment)
    {
        _vehicleService = vehicleService;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpPost("CreateVehicle")]
    public async Task<IActionResult> AddVehicle([FromForm]CreateVehicleDto model)
    { 
        if(ModelState.IsValid)
        {
            if(model.File==null || model.File.Length == 0)
            {
                return BadRequest("File is required");
            }
            string uploadPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images");
            string fileName= $"{Guid.NewGuid()}{Path.GetExtension(model.File.FileName)}";
            string filePath = Path.Combine(uploadPath, fileName);
            
            model.Image = fileName;

            var result = await _vehicleService.CreateVehicle(model);
            if (result.isSuccess)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(fileStream);
                }
                return Ok(result);
            }
        }
        return BadRequest();
    }

    [HttpGet("GetVehicle")]
    public async Task<IActionResult> GetAllVehicle()
    { 
      var vehicles= await _vehicleService.GetVehicle();
      return Ok(vehicles);
    }

    [HttpPut("UpdateVehicle")]
    public async Task<IActionResult> UpdateVehicle(int vehicleId, [FromForm]UpdateVehicleDto model)
    {
        if (ModelState.IsValid)
        {
            var result = await _vehicleService.UpdateVehicleResponse(vehicleId, model);
            if (result.isSuccess)
            {
                return Ok(result);
            }
        }
        return BadRequest();
    }

    [Authorize(Roles = "Administrator")]
    [HttpDelete("Remove/Vehicle/{vehicleId}")]
    public async Task<IActionResult> DeleteVehicle([FromRoute] int vehicleId)
    {
        var result = await _vehicleService.DeleteVehicle(vehicleId);
        if (result.isSuccess)
        {
            return Ok(result);
        }
        return BadRequest();
    }

    [HttpGet("{vehicleId}")]
    public async Task<IActionResult> GetVehicleById([FromRoute] int vehicleId)
    {
        var result = await _vehicleService.GetVehicleById(vehicleId);
        if (result.isSuccess)
        {
            return Ok(result);
        }
        return BadRequest();
    }

    [HttpPut("{vehicleId}")]
    public async Task<IActionResult> ChangeStatus([FromRoute] int vehicleId)
    {
        var result = await _vehicleService.ChangeVehicleStatus(vehicleId);
        if (result.isSuccess)
        {
            return Ok(result);
        }
        return BadRequest();
    }
} 