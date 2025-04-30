using Galaxy_Auction_Data_Access.Enums;
using Galaxy_Auction_Data_Access.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Galaxy_Auction_Data_Access.Domain;

public class Bid
{
    [Key]
    public int BidId { get; set; }
    public decimal BidAmount { get; set; }
    public DateTime BidDate { get; set; }
    public string BidStatus { get; set; } = Galaxy_Auction_Data_Access.Enums.BidStatus.Pending.ToString();
    public string UserId { get; set; }
    [JsonIgnore]
    public ApplicationUser User { get; set; }
    public int VehicleId { get; set; }
    [JsonIgnore]
    public Vehicle Vehicle { get; set; }

}
