namespace GrpcServer.Data;

public class ChatMessageRecord
{
    public int Id { get; set; } 
    
    public string User { get; set; } = string.Empty;
    
    public string Text { get; set; } = string.Empty;
    
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}