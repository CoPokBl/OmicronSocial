using GeneralPurposeLib;
using MySql.Data.MySqlClient;
using OmicronSocial.Data.Schemas;

namespace OmicronSocial.Data.Storage;

public class MySqlStorage : IStorageManager {
    
    private string _connectionString = "";
    
    public void Init() {
        Logger.Info($"[MySQL] Connecting to {GlobalConfig.Config["mysql_host"].Text} as {GlobalConfig.Config["mysql_user"].Text}...");
        _connectionString = $"server={GlobalConfig.Config["mysql_host"].Text};" +
                            $"userid={GlobalConfig.Config["mysql_user"].Text};" +
                            $"password={GlobalConfig.Config["mysql_pass"].Text};" +
                            $"database={GlobalConfig.Config["mysql_db"].Text}";
        CreateTables().Wait();
        Logger.Info("Initialised MySQL");
    }
    
    private async Task CreateTables() {
        await SendMySqlStatement(@"CREATE TABLE IF NOT EXISTS users(
                           id VARCHAR(64) primary key,
                           session_id VARCHAR(64),
                           refresh_token VARCHAR(512),
                           is_banned BOOLEAN)");
    }
    
    private async Task SendMySqlStatement(string statement) {
        await MySqlHelper.ExecuteNonQueryAsync(_connectionString, statement);
    }

    public Task CreateUser(StoredUser user) {
        return SendMySqlStatement($"INSERT INTO users (id, session_id, refresh_token, is_banned) VALUES ('{user.Id}', '{user.SessionId}', '{user.RefreshToken}', {user.IsBanned})");
    }

    public Task UpdateUser(StoredUser user) {
        return SendMySqlStatement($"UPDATE users SET session_id = '{user.SessionId}', refresh_token = '{user.RefreshToken}', is_banned = {user.IsBanned} WHERE id = '{user.Id}'");
    }

    public Task DeleteUser(StoredUser user) {
        return SendMySqlStatement($"DELETE FROM users WHERE id = '{user.Id}'");
    }

    public async Task<StoredUser?> GetUser(string userId) {
        StoredUser? user;
        await using MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(_connectionString, $"SELECT * FROM users WHERE id='{userId}'");
        if (!reader.Read()) {
            user = null;
            return user;
        }
        user = new StoredUser {
            Id = reader.GetString("id"),
            SessionId = reader.GetString("session_id"),
            RefreshToken = reader.GetString("refresh_token"),
            IsBanned = reader.GetBoolean("is_banned")
        };
        reader.Close();
        return user;
    }
    
}