namespace OmicronSocial.Data.Schemas; 

/// <summary>
/// The schema of a user in the database.
/// </summary>
public struct StoredUser {
    public string Id;
    public string SessionId;
    public string RefreshToken;
    public bool IsBanned;
}