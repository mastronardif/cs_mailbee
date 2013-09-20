using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using System.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace myrabbitmq
{
    static class myRabbitMQVitals
    {
        public static string _queue = ConfigurationManager.AppSettings["RabbitMQqueue"];
        public static string _exchange = ConfigurationManager.AppSettings["RabbitMQexchange"].ToString();
        public static string _routing_key = ConfigurationManager.AppSettings["RabbitMQrouting_key"].ToString();
    }

    class MyRabbotMQ
    {
        static public string Publish(string message)
        {
            string retval = string.Empty;

            // First we need a ConnectionFactory
            ConnectionFactory connFactory = new ConnectionFactory
            {
                // AppSettings["CLOUDAMQP_URL"] contains the connection string
                // when you've added the CloudAMQP Addon
                Uri = ConfigurationManager.AppSettings["CLOUDAMQP_URL"]
            };

            //ConnectionFactory factory = new ConnectionFactory();
            //factory.HostName = "localhost";
            //using (IConnection connection = factory.CreateConnection())
            using (var conn = connFactory.CreateConnection())
            {
                //using (IModel channel = connection.CreateModel())
                using (var channel = conn.CreateModel()) // Note, don't share channels between threads
                {
                    helperChannelDeclareBind(channel);
                    byte[] body = System.Text.Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(myRabbitMQVitals._exchange, myRabbitMQVitals._routing_key, null, body);
                }
            }

            return retval;
        }

        static public string Pop()
        { 
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";
            bool noAck = false;
            string message = string.Empty;

            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    helperChannelDeclareBind(channel);
                    BasicGetResult result = channel.BasicGet(myRabbitMQVitals._queue, noAck);

                    if (result == null) {
                        // No message available at this time.
                        message = "// No message available at this time.";
                    } 
                    else 
                    {
                        IBasicProperties props = result.BasicProperties;
                        byte[] body = result.Body;
                        message = System.Text.Encoding.UTF8.GetString(body);

                        // acknowledge receipt of the message {true - you can remove it.}
                        channel.BasicAck(result.DeliveryTag, true);
                    }

                    return (message);
                }
            }
        }

        static private void helperChannelDeclareBind(IModel channel)
        {
            channel.QueueDeclare(myRabbitMQVitals._queue, false, false, false, null);
            channel.ExchangeDeclare(myRabbitMQVitals._exchange, RabbitMQ.Client.ExchangeType.Direct);
            channel.QueueBind(myRabbitMQVitals._queue, myRabbitMQVitals._exchange, myRabbitMQVitals._routing_key);
        }

        static public void my_XslXmlOut(string fnXml, string fnXsl, string fnOut)
        {
            XslCompiledTransform myXslTransform;
            myXslTransform = new XslCompiledTransform();
            myXslTransform.Load(fnXsl);
            myXslTransform.Transform(fnXml, fnOut); 
        }

        static public T Deserialize<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var stringReader = new StringReader(xml))
            {
                T retval = (T)serializer.Deserialize(stringReader);

                return retval;
            }
        }
    }
}
