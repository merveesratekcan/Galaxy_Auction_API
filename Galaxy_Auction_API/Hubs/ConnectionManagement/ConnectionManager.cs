
namespace Galaxy_Auction_API.Hubs.ConnectionManagement;

public class ConnectionManager : IConnectionManager
{
    public static Dictionary<string, List<string> > _userConnection= new Dictionary<string, List<string>>();
   
    
    public void AddConnection(string userId, string connectionId)
    {
       lock (_userConnection)
        {
            if (_userConnection.ContainsKey(userId))
            {
                _userConnection[userId].Add(connectionId);
            }
            else
            {
                _userConnection[userId] = new List<string> { connectionId };
            }
        }
    }

    public List<string> GetAllConnectionIds()
    {
        return _userConnection.Values.SelectMany(connections => connections).ToList();
    }

    public string GetConnectionId(string userId)
    {
        lock (_userConnection)
        {
          return _userConnection.ContainsKey(userId) ?
                _userConnection[userId].FirstOrDefault() : null;

        }

        

    }

    public IEnumerable<string> GetConnections(string userId)
    {
        lock (_userConnection)
        {
          return _userConnection.ContainsKey(userId) ? 
                _userConnection[userId] : Enumerable.Empty<string>();
        }
    }

    public List<string> GetSpecificConnestions()
    {
        var result = _userConnection.Values;
        return new List<string>();
    }

    public void RemoveConnection(string connectionId)
    {
        lock (_userConnection)
        {
            foreach(var userId in _userConnection.Keys)
            {
                _userConnection[userId].Remove(connectionId);
            }
        }
    }
}
