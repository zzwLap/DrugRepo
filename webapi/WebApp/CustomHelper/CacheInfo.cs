using Dapper;
using Microsoft.Data.Sqlite;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using webapi.Models;

namespace webapi;

public class CacheInfo
{
    private static string connectionString;
    private static ConcurrentDictionary<int, LoginInfoCache> UserCache { get; set; } = new ConcurrentDictionary<int, LoginInfoCache>();
    static CacheInfo() { }

    public CacheInfo(IConfiguration config)
    {
        CacheInfo.connectionString = config.GetConnectionString("MyContext");
    }

    public async ValueTask<LoginInfoCache> GetUserAsync(int userId, IDbConnection? conn = null, IDbTransaction? trans = null, bool refresh = false)
    {
        if (refresh)
        {
            return await ExecGetUserAsync(userId, conn, trans);
        }
        if (UserCache.TryGetValue(userId, out var user) && user != null && DateTime.Now > user.LossTime)
        {
            return user;
        }
        return await ExecGetUserAsync(userId, conn, trans);
    }

    public async ValueTask<LoginInfoCache> RefreshUser(int userId, IDbConnection? conn = null, IDbTransaction? trans = null)
    {
        return await ExecGetUserAsync(userId, conn, trans);
    }
    private async Task<LoginInfoCache> ExecGetUserAsync(int userId, IDbConnection? conn = null, IDbTransaction? trans = null)
    {
        var currentConn = conn;
        if (currentConn == null)
        {
            currentConn = GetDbConnection();
        }
        var user = await currentConn.QueryFirstAsync<LoginInfoCache>("select UserId,UserName from User where UserId=@userId", new { userId = userId }, transaction: trans);
        user.LossTime = DateTime.Now.AddSeconds(2);
        UserCache[userId] = user;
        return user;
    }

    public static IDbConnection GetDbConnection()
    {
        return new SqliteConnection(connectionString);
    }
}

public class LoginInfoCache
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public DateTime LossTime { get; set; }
}
