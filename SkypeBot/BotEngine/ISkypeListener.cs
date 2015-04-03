namespace SkypeBot.BotEngine
{
    public delegate void SkypeMessageHandler(string source, string message);
    public delegate void FoundContactHandler(string contact);
    public interface ISkypeListener
    {
        event SkypeMessageHandler SkypeMessageReceived;
        event FoundContactHandler FoundNewContact;
        void Initialize();
    }
}
