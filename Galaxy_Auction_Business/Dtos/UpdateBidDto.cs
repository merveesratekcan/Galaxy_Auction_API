using Galaxy_Auction_Data_Access.Domain;
using Galaxy_Auction_Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Auction_Business.Dtos;

public class UpdateBidDto
{
    public decimal BidAmount { get; set; }
    public DateTime BidDate { get; set; }
    public string BidStatus { get; set; } 
    public string UserId { get; set; }
    public int VehicleId { get; set; }
}
