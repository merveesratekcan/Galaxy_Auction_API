using Galaxy_Auction_Core.Comman;
using Galaxy_Auction_Data_Access.Context;
using Galaxy_Auction_Data_Access.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Galaxy_Auction_API.Extensions;

public static class OptionsExt
{
    public static IServiceCollection AddInfractructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StripeSettings>(options=>configuration.GetSection("StripeSettings").Bind(options));
        return services;
    }
} 