using SkypeBot.SkypeDB;

namespace SkypeBot.BotEngine
{
    public delegate void SkypeMessageHandler(string source, SkypeMessage message);
    public delegate void FoundContactHandler(string contact);
    public interface ISkypeListener
    {
        event SkypeMessageHandler SkypeMessageReceived;
        event FoundContactHandler FoundNewContact;
        void Initialize();
    }
}
