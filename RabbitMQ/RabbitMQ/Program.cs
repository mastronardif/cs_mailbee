using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using log4net.Config;
using log4net;
using System.Threading;

namespace RabbitMQ
{
    static class myRabbitMQVitals
    {
        public static string _queue = ConfigurationManager.AppSettings["RabbitMQqueue"];
        public static string _exchange = ConfigurationManager.AppSettings["RabbitMQexchange"].ToString();
        public static string _routing_key = ConfigurationManager.AppSettings["RabbitMQrouting_key"].ToString();
    }
    delegate string delReply(string msg);
    class Program
    {
        static public readonly ILog _log = LogManager.GetLogger(typeof(Program));

        

        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            string op = (args.Length > 0) ? args[0] : "receive"; //string.Join(", ", args);
            string retval = string.Empty;
            Console.WriteLine(op);

            //op = "pop";
            switch (op.ToLower())
            {
                case "pop":
                    retval = RabbitMQ.Business.busMail.popOne();
                    Console.WriteLine("Pop action. " +  retval);
                    _log.Debug(retval);
                    break;

                case "read":
                case "receive":
                    Console.WriteLine("Receive action.  I read.");
                    //Receive.MainReceive(args);
                    Receive.MainReceive(new delReply(RabbitMQ.Business.busMail.Reply));
                    break;

                case "send":
                    Console.WriteLine("Send action.  I did send.");
                    //Send.MainSend(args);
                    string message = (args.Length > 1) ? args[1] : "Hello World!";
                    RabbitMQ.Business.busMail.publish(message);
                    break;


                case "testjoemailweb":
                case "testjoeping":
                    test_(op);
                    break;
            
                default:
                    Console.WriteLine("unkown action.  I did nothing.\n" +
                                       "Choices: \n" + 
                                       "pop\nread or receive\nsend\n" + 
                                       "______________________\n"+
                                       "testJoemailweb\ntestJoeping\n");

                break;
            }
        }


        // Test
    
        public static void test_(string strKey)
        {
            string retval = string.Empty;
            if (strKey.IndexOf("joemailweb", StringComparison.OrdinalIgnoreCase) != -1)
            {
                retval = MvcApplication2.Models.TestDataClass1.readTestFile("Testjoemailweb.txt");
            }
            else if (strKey.IndexOf("joeping", StringComparison.OrdinalIgnoreCase) != -1)
            {
                retval = MvcApplication2.Models.TestDataClass1.readTestFile("Testjoeping.txt");
            }
            
            //new string[] args = {"", ""};
            
            string[] args = new string[] { "send", retval };

            // push
            Main(args);

            // wait
            Thread.Sleep(5 * 1000);
            Console.WriteLine("Press any key to pop.");
            Console.ReadLine();

            args[0] = "pop"; 
            args[1] = "you buddy";
            // pop
            Main(args);
        }
    }


    class Receive
    {
        //public static void MainReceive(string[] args)            
        public static void MainReceive(delReply reply)
        {
            //ConnectionFactory factory = new ConnectionFactory();
            //factory.HostName = "localhost";

            // First we need a ConnectionFactory
            ConnectionFactory connFactory = new ConnectionFactory
            {
                // AppSettings["CLOUDAMQP_URL"] contains the connection string
                // when you've added the CloudAMQP Addon
                Uri = ConfigurationManager.AppSettings["CLOUDAMQP_URL"]
            };




            //using (IConnection connection = factory.CreateConnection())
            //using (IModel channel = connection.CreateModel())
            using (var conn = connFactory.CreateConnection())
            {
                using (var channel = conn.CreateModel()) // Note, don't share channels between threads
                {
                    channel.QueueDeclare(myRabbitMQVitals._queue, false, false, false, null);

                    QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(myRabbitMQVitals._queue, true, consumer);

                    System.Console.WriteLine(" [*] Waiting for messages." +
                                             "To exit press CTRL+C");
                    while (true)
                    {
                        BasicDeliverEventArgs ea =
                            (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        byte[] body = ea.Body;

                        string message = System.Text.Encoding.UTF8.GetString(body);
                        System.Console.WriteLine(" [x] Received {0}", message);

                        reply(message);

                    }
                }
            }
        }
    }


    class Send
    {
        public static void MainSend(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(myRabbitMQVitals._queue, false, false, false, null);
                    channel.ExchangeDeclare(myRabbitMQVitals._exchange,  RabbitMQ.Client.ExchangeType.Direct);
                    channel.QueueBind(myRabbitMQVitals._queue, myRabbitMQVitals._exchange, myRabbitMQVitals._routing_key);

                    string message = (args.Length > 1) ? args[1] : "Hello World!";
                    byte[] body = System.Text.Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(myRabbitMQVitals._exchange, myRabbitMQVitals._routing_key, null, body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
        }
    }
}
