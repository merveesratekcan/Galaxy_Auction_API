using Galaxy_Auction_API.Hubs.ConnectionManagement;
using Galaxy_Auction_Business.Dtos;
using Galaxy_Auction_Data_Access.Context;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Galaxy_Auction_API.Hubs;

public class BidUpdateHub:Hub
{
    private readonly IConnectionManager _connectionManager;
    private readonly ApplicationDbContext _context;
    public BidUpdateHub(IConnectionManager connectionManager, ApplicationDbContext context)
    {
        _connectionManager = connectionManager;
        _context = context;
    }

    public override async Task OnConnectedAsync()
    {
        Random random = new Random();
        int randomNumber = random.Next(1, 1000);
        var connectionId = Context.ConnectionId;
        var userId =  randomNumber.ToString();
        _connectionManager.AddConnection(userId, connectionId);
        await base.OnConnectedAsync();

    }
    public async Task NewBid(int vehicleId)
    {
        if (vehicleId != null)
        {
            var result = await _context.Bids
                        .Where(b => b.VehicleId == vehicleId).ToListAsync();
            Random random = new Random();
            int randomNumber = random.Next(1, 1000);
            var connedtionId = _connectionManager.GetAllConnectionIds();
            await Clients.Clients(connedtionId).SendAsync("messageReceived", result);
        }
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        _connectionManager.RemoveConnection(connectionId);
        return base.OnDisconnectedAsync(exception);
    }


}

