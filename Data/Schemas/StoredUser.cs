namespace OmicronSocial.Data.Schemas; 

public struct StoredUser {
    public string Id;
    public string SessionId;
    public string RefreshToken;
    public bool IsBanned;
}