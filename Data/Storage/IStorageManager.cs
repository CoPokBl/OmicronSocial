using OmicronSocial.Data.Schemas;

namespace OmicronSocial.Data.Storage; 

public interface IStorageManager {

    void Init();
    void Deinit();

    Task CreateUser(StoredUser user);
    Task UpdateUser(StoredUser user);
    Task DeleteUser(StoredUser user);
    Task<StoredUser?> GetUser(string userId);

}