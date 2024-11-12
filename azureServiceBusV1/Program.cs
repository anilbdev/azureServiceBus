using Azure.Messaging.ServiceBus;

namespace azureServiceBusV1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            //create a connectiom
            await using var client = new ServiceBusClient("xxxxxxx");//TODO add connection string
            //cretae sender
            ServiceBusSender sender = client.CreateSender("sb-queue-v2");

            //send message
            ServiceBusMessage message = new ServiceBusMessage("Hello, Bob!,");
            await sender.SendMessageAsync(message);

            //create reciever

            ServiceBusReceiver receiver = client.CreateReceiver("sb-queue-v2");


            //recieve message
            ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

            //sending message to dlq
            await receiver.DeadLetterMessageAsync(receivedMessage,"reason","description");

            //receiver message form dlq

            ServiceBusReceiver dlqrec = client.CreateReceiver("sb-queue-v2", new ServiceBusReceiverOptions { SubQueue = SubQueue.DeadLetter });

            ServiceBusReceivedMessage dlqmsg = await dlqrec.ReceiveMessageAsync();


            //dead letter receievr
            ServiceBusReceiver dlreceievr = client.CreateReceiver("sb-queue-v2",new ServiceBusReceiverOptions { SubQueue = SubQueue.DeadLetter});


            Console.WriteLine(dlqmsg.Body.ToString());
            Console.ReadLine();
        }
    }
}
