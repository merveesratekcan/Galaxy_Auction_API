using Galaxy_Auction_Business.Abstraction;
using Galaxy_Auction_Business.Concrete;
using Galaxy_Auction_Core.MailHelper;
using Galaxy_Auction_Core.Models;

namespace Galaxy_Auction_API.Extensions;

public static class ServiceCollectionExt
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IBidService, BidService>();
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<IPaymentHistoryService,PaymentHistoryService>();
        services.AddScoped(typeof(ApiResponse));
        return services;
    }
} 