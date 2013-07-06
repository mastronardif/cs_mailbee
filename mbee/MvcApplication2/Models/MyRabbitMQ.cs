﻿using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;
using System.Configuration;

namespace myrabbitmq
{
    static class myRabbitMQVitals
    {
        //public static string _queue = System.Web.Configuration.WebConfigurationManager.AppSettings["RabbitMQqueue"].ToString();
        //public static string _exchange = System.Web.Configuration.WebConfigurationManager.AppSettings["RabbitMQexchange"].ToString();
        //public static string _routing_key = System.Web.Configuration.WebConfigurationManager.AppSettings["RabbitMQrouting_key"].ToString();

        public static string _queue = ConfigurationManager.AppSettings["RabbitMQqueue"];
        public static string _exchange = ConfigurationManager.AppSettings["RabbitMQexchange"].ToString();
        public static string _routing_key = ConfigurationManager.AppSettings["RabbitMQrouting_key"].ToString();
    }

    class MyRabbotMQ
    {
        static public string Publish(string message)
        {
            string retval = string.Empty;
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";

            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(myRabbitMQVitals._queue, false, false, false, null);
                    channel.ExchangeDeclare(myRabbitMQVitals._exchange, RabbitMQ.Client.ExchangeType.Direct);
                    channel.QueueBind(myRabbitMQVitals._queue, myRabbitMQVitals._exchange, myRabbitMQVitals._routing_key);

                    byte[] body = System.Text.Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(myRabbitMQVitals._exchange, myRabbitMQVitals._routing_key, null, body);
                }
            }

            return retval;
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
