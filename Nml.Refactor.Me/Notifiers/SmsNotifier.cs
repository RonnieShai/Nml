using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Nml.Refactor.Me.Dependencies;
using Nml.Refactor.Me.MessageBuilders;

namespace Nml.Refactor.Me.Notifiers
{
	public class SmsNotifier : INotifier
	{
		private readonly IStringMessageBuilder _messageBuilder;
		private readonly IOptions _options;
		private readonly ILogger _logger = LogManager.For<SmsNotifier>();

		public SmsNotifier(IStringMessageBuilder messageBuilder, IOptions options)
		{
			_messageBuilder = messageBuilder ?? throw new ArgumentNullException(nameof(messageBuilder));
			_options = options ?? throw new ArgumentNullException(nameof(options));
		}
		
		public async Task Notify(NotificationMessage message)
		{
			//Complete after refactoring inheritance. Use "SmsApiClient"

			var serviceEndPoint = new Uri(_options.Sms.ApiUri);

			SmsApiClient client = new SmsApiClient(_options.Sms.ApiUri, _options.Sms.ApiKey);
			var request = new HttpRequestMessage(HttpMethod.Post, serviceEndPoint);
            request.Content = new StringContent(
                _messageBuilder.CreateMessage(message).ToString(),
                Encoding.UTF8,
                "application/json");

            try
            {
                await client.SendAsync(message.To, request.ToString());
				_logger.LogTrace($"Message sent.");
			}
            catch (AggregateException e)
            {
                foreach (var exception in e.Flatten().InnerExceptions)
                    _logger.LogError(exception, $"Failed to send message. {exception.Message}");

                throw;
            }

        }
	}
}
