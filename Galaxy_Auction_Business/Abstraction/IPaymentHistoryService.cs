using Galaxy_Auction_Business.Dtos;
using Galaxy_Auction_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Auction_Business.Abstraction;

public interface IPaymentHistoryService
{
    Task<ApiResponse> CreatePaymentHistory(CreatePaymentHistoryDto model);

    //ilgili arac veri tabaninda aktif olmali
    Task<ApiResponse> CheckIsStatusForAuction(string userId,int vehicleId);
}
