﻿namespace SkypeBot.BotEngine
{
    public delegate void SkypeMessageHandler(string source, string message);
    public interface ISkypeListener
    {
        event SkypeMessageHandler SkypeMessageReceived;
        void Initialize();
    }
}
