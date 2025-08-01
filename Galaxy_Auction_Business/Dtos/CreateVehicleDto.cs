﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Auction_Business.Dtos;

public class CreateVehicleDto
{
    public string BrandAndModel { get; set; }
    public int ManufacturingYear { get; set; }
    public string Color { get; set; }
    public decimal EngineCapacity { get; set; }
    public decimal Price { get; set; }
    public int Mileage { get; set; }
    public string PlateNumber { get; set; }
    public double AuctionPrice { get; set; }
    public string AdditionalInformation { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsActive { get; set; }
    public string Image { get; set; }
    public string SellerId { get; set; }
    public IFormFile File { get; set; }
}
