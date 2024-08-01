using System.Collections.Concurrent;
using System.Security.Cryptography;
using OmicronSocial.Data.Schemas;

namespace OmicronSocial.Data;

public static class Chats {
    private static readonly ConcurrentQueue<(int, Action<Chat>)> ChatQueue = new();
    private static readonly ConcurrentBag<Chat> OngoingChats = [];
    private static CancellationToken _cancellationToken;

    public static void Init(CancellationToken token) {
        _cancellationToken = token;
        Thread connectorThread = new(ConnectorLoop);
        connectorThread.Start();
    }
    
    public static void Queue(int id, Action<Chat> callback) {
        ChatQueue.Enqueue((id, callback));
    }

    private static void ConnectorLoop() {
        while (!_cancellationToken.IsCancellationRequested) {
            if (ChatQueue.Count < 2) {
                try {
                    Task.Delay(1000, _cancellationToken).Wait();
                }
                catch (TaskCanceledException) {
                    return;
                }
                continue;
            }
            
            // Join them
            int chatId = RandomNumberGenerator.GetInt32(1000000);
            
            ChatQueue.TryDequeue(out (int, Action<Chat>) u1);
            ChatQueue.TryDequeue(out (int, Action<Chat>) u2);
            
            Chat chat = new(chatId) { Peer1 = u1.Item1, Peer2 = u2.Item1};
            
            u1.Item2(chat);
            u2.Item2(chat);
            
            OngoingChats.Add(chat);
            
            Console.WriteLine($"Placed {u1.Item1} and {u2.Item1} in a chat");
        }
    }
}