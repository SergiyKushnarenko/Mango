using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.Services.PaymentAPI.Messages;
using Newtonsoft.Json;
using PaymentProcessor;
using System.Text;

namespace Mango.Services.PaymentAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string subscriptionPayment;
        private readonly string orderPaymentProcessTopic;
        private readonly string orderupdatepaymentresulttopic;


        private ServiceBusProcessor orderPaymnetProcessor;
        private readonly IProcessPaymant _processPaymant;
        private readonly IConfiguration _configuration;
        private readonly IMessageBus _messageBus;
        public AzureServiceBusConsumer(IConfiguration configuration, IMessageBus messageBus, IProcessPaymant processPaymant)
        {
            _configuration = configuration;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionPayment = _configuration.GetValue<string>("OrderPaymentProcessSubscription");
            orderupdatepaymentresulttopic = _configuration.GetValue<string>("OrderUpdatePaymentResultTopic");
            orderPaymentProcessTopic = _configuration.GetValue<string>("OrderPaymentProcessTopic");

            var client = new ServiceBusClient(serviceBusConnectionString);

            orderPaymnetProcessor = client.CreateProcessor(orderPaymentProcessTopic, subscriptionPayment);
            _messageBus = messageBus;
            _processPaymant = processPaymant;
        }

        public async Task Start()
        {
            orderPaymnetProcessor.ProcessMessageAsync += ProcessPayments;
            orderPaymnetProcessor.ProcessErrorAsync += ErrorHandler;
            await orderPaymnetProcessor.StartProcessingAsync();

           
        }
        public async Task Stop()
        {
            await orderPaymnetProcessor.StopProcessingAsync();
            await orderPaymnetProcessor.DisposeAsync();
        }
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }


        private async Task ProcessPayments(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            PaymentRequestMessage paymentRequestMessage = JsonConvert.DeserializeObject<PaymentRequestMessage>(body);

            var result = _processPaymant.PaymentProcessor();

            UpdatePaymentResultMessage updatePaymentResultMessage = new()
            {
                Status = result,
                OrderId = paymentRequestMessage.OrderId
            };

            try
            {
                await _messageBus.PublichMessage(updatePaymentResultMessage, orderupdatepaymentresulttopic);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
