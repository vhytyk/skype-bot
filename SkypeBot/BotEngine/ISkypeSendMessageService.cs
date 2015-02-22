namespace SkypeBot.BotEngine
{
    public interface ISkypeSendMessageService
    {
        void SendMessage(string contact, string message);
    }
}
