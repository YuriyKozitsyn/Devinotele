using Infrastructure;
using NLog;
using Scheduler.Providers;
using System;
using System.Linq;
using System.Timers;
using ILogger = Infrastructure.ILogger;

namespace Scheduler
{
    public class Scheduler : StartableService
    {
        private readonly MessageDataProvider _messageDataProvider;
        private readonly ILogger _logger;

        public Scheduler(
            MessageDataProvider messageDataProvider,
            ILogger logger)
        {
            _messageDataProvider = messageDataProvider;
            _logger = logger;
        }

        public static void Main()
        {
            Run(new Scheduler(new MessageDataProvider(), new NLogLogger(LogManager.CreateNullLogger())));
        }

        public override void Start()
        {
            OnTimedEvent(null, null);
           //var aTimer = new Timer(TimeSpan.FromSeconds(2).TotalMilliseconds);
           // aTimer.Elapsed += OnTimedEvent;
           // aTimer.AutoReset = true;
           // aTimer.Enabled = true;
        }

        private void OnTimedEvent(object s, ElapsedEventArgs e)
        {
            

            var messagesByDate = _messageDataProvider.GetMessagesByDate(DateTime.Now);

            _logger.Debug($"Получили {messagesByDate.Count()} сообщений");
            
            var sender = new Sender(_logger);
            foreach (var message in messagesByDate)
            {
                sender.Send(message);
            }

            _messageDataProvider.DeleteMessages(messagesByDate);
        }

        public override void OnError(Exception exception)
        {
            Console.Error.WriteLine("Expected");
            _logger.Fatal($"Произошла ошибка во время выполнения метода {nameof(Start)}");
        }
    }
}
