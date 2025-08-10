namespace Galaxy_Auction_API.Hubs.ConnectionManagement;

public interface IConnectionManager
{
    void AddConnection(string userId, string connectionId);
    string GetConnectionId(string userId);
    IEnumerable<string> GetConnections(string userId);
    void RemoveConnection(string connectionId);
    List<string> GetAllConnectionIds();
    List<string> GetSpecificConnestions();

}
