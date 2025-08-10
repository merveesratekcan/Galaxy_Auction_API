using Galaxy_Auction_API.Hubs.ConnectionManagement;
using Galaxy_Auction_Business.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace Galaxy_Auction_API.Hubs;

public class BidUpdateHub:Hub
{
    private readonly IConnectionManager _connectionManager;
    public BidUpdateHub(IConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
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
    public async Task NewBid()
    {
        await Clients.Clients(new List<string>).SendAsync("messageReceived","deneme");
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        _connectionManager.RemoveConnection(connectionId);
        return base.OnDisconnectedAsync(exception);
    }


}

