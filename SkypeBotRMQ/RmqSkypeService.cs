using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SkypeBotRMQ
{
    public class RmqSkypeService : IRmqSkypeService
    {
        private const string skypeMessageQueue = "skypeQueue";
        private string Serialize(RmqSkypeMessage message)
        {
            var serializer = new DataContractJsonSerializer(typeof(RmqSkypeMessage));
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, message);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        private RmqSkypeMessage DeSerialize(string message)
        {
            var serializer = new DataContractJsonSerializer(typeof(RmqSkypeMessage));
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(message)))
            {
                return (RmqSkypeMessage)serializer.ReadObject(ms);
            }
        }

        public void PushMessage(RmqSkypeMessage skypeMessage)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(skypeMessageQueue, false, false, false, null);
                    string serializedBody = Serialize(skypeMessage);
                    var body = Encoding.UTF8.GetBytes(serializedBody);
                    channel.BasicPublish(string.Empty, skypeMessageQueue, null, body);
                }
            }
        }

        public RmqSkypeMessage PullMessage()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(skypeMessageQueue, false, false, false, null);
                    BasicGetResult result = channel.BasicGet(skypeMessageQueue, true);
                    if (result != null)
                    {
                        var body = result.Body;
                        var message = Encoding.UTF8.GetString(body);
                        return DeSerialize(message);
                    }
                    return null;
                }
            }
        }
    }
}
