using Galaxy_Auction_Data_Access.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Auction_Data_Access.Domain;

public class PaymentHistory
{
    [Key]
    public int PaymentId { get; set; }
    public string UserId { get; set; }
    public string ClientSecret { get; set; }
    public string StripePaymentIntentId { get; set; }
    //Birebir ilişki
    public ApplicationUser User { get; set; }
    public bool IsActive { get; set; }
    public DateTime PayDate { get; set; }
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }

}
