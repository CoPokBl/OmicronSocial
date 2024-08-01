namespace OmicronSocial.Data.Schemas;

public class Chat(int Id) {
    public int Peer1;
    public int Peer2;
    private event Action<int, string>? MessageSent;
    private object _lock = new();
    
    public void Subscribe(Action<int, string> callback) {
        lock (_lock) {
            MessageSent += callback;
        }
    }
    
    public void SendMessage(int peer, string message) {
        MessageSent?.Invoke(peer, message);
    }

    public int NotMe(int me) {
        return Peer1 == me ? Peer2 : Peer1;
    }
}
