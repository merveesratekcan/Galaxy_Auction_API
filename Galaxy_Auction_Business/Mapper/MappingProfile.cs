using AutoMapper;
using Galaxy_Auction_Business.Dtos;
using Galaxy_Auction_Data_Access.Domain;
using Galaxy_Auction_Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Auction_Business.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterRequestDto, ApplicationUser>().ReverseMap();
        CreateMap<CreateVehicleDto, Vehicle>().ReverseMap();
        CreateMap<UpdateVehicleDto, Vehicle>().ReverseMap();
        CreateMap<UpdateBidDto, Bid>().ReverseMap();
        CreateMap<CreateBidDto, Bid>().ReverseMap();
        CreateMap<CreatePaymentHistoryDto, PaymentHistory>().ReverseMap();




    }
}
