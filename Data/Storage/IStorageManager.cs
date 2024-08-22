using OmicronSocial.Data.Schemas;

namespace OmicronSocial.Data.Storage; 

public interface IStorageManager {

    /// <summary>
    /// Initialise the storage manager.
    /// </summary>
    void Init();

    /// <summary>
    /// Add a user to the database.
    /// </summary>
    /// <param name="user">The user to add.</param>
    /// <returns>A task which completes once the user has been added.</returns>
    Task CreateUser(StoredUser user);
    
    /// <summary>
    /// Modify the properties of a user in the database.
    /// </summary>
    /// <param name="user">The user to modify with the properties modified, the ID should remain the same.</param>
    /// <returns>A task which completes once the user has been updated.</returns>
    Task UpdateUser(StoredUser user);
    
    /// <summary>
    /// Remove a user from the database.
    /// </summary>
    /// <param name="user">The user to remove.</param>
    /// <returns>A task which completes when the user has been removed.</returns>
    Task DeleteUser(StoredUser user);
    
    /// <summary>
    /// Gets a user from the database.
    /// </summary>
    /// <param name="userId">The ID of the user to get.</param>
    /// <returns>
    /// A task which completes once the user has been retrieved with the user object
    /// or null if the user could not be found.
    /// </returns>
    Task<StoredUser?> GetUser(string userId);

}