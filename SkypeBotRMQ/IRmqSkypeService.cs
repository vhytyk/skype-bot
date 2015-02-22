using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SkypeBotRMQ
{
    public interface IRmqSkypeService
    {
        void PushMessage(RmqSkypeMessage skypeMessage);
        RmqSkypeMessage PullMessage();
    }
}
