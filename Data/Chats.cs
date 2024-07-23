using System.Collections.Concurrent;
using System.Security.Cryptography;
using OmicronSocial.Data.Schemas;

namespace OmicronSocial.Data;

public static class Chats {
    private static readonly ConcurrentQueue<(int, Action<Chat>)> ChatQueue = new();
    private static readonly List<Chat> OngoingChats = [];

    public static void Init() {
        Thread connectorThread = new(ConnectorLoop);
        connectorThread.Start();
    }
    
    public static void Queue(int id, Action<Chat> callback) {
        ChatQueue.Enqueue((id, callback));
    }

    private static void ConnectorLoop() {
        while (true) {
            if (ChatQueue.Count < 2) {
                Thread.Sleep(1000);
                continue;
            }
            
            // Join them
            int chatId = RandomNumberGenerator.GetInt32(1000000);
            
            ChatQueue.TryDequeue(out (int, Action<Chat>) u1);
            ChatQueue.TryDequeue(out (int, Action<Chat>) u2);
            
            Chat u1Chat = new(chatId) { Peer = u2.Item1 };
            Chat u2Chat = new(chatId) { Peer = u1.Item1 };
            
            u1.Item2(u1Chat);
            u2.Item2(u2Chat);
            
            Console.WriteLine($"Placed {u1.Item1} and {u2.Item1} in a chat");
        }
    }
}