﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkypeBot.BotEngine
{
    public interface IRmqListener : IDisposable
    {
        void Initialize();
        event SkypeMessageHandler SkypeMessageReceived;
    }
}
