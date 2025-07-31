using Galaxy_Auction_Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Galaxy_Auction_Data_Access.Domain;

public class Vehicle
{
    public int VehicleId { get; set; }
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

    [JsonIgnore]
    
    public ApplicationUser Seller { get; set; }

    public virtual List<Bid> Bids { get; set; }
    public Vehicle()
    {
        Bids = new List<Bid>();
    }

}
