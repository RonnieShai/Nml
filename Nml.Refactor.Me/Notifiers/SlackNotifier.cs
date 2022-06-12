using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Nml.Refactor.Me.Dependencies;
using Nml.Refactor.Me.MessageBuilders;

namespace Nml.Refactor.Me.Notifiers
{
	public class SlackNotifier : INotifier
	{
		private readonly IWebhookMessageBuilder _messageBuilder;
		private readonly IOptions _options;
		private readonly ILogger _logger = LogManager.For<SlackNotifier>();

		public SlackNotifier(IWebhookMessageBuilder messageBuilder, IOptions options)
		{
			_messageBuilder = messageBuilder ?? throw new ArgumentNullException(nameof(messageBuilder));
			_options = options ?? throw new ArgumentNullException(nameof(options));
		}

		public async Task Notify(NotificationMessage message)
		{
			await SendHttpClientMessage.SendMessageAsync(_options.Slack.WebhookUri, message, _messageBuilder, _logger);
		}
	}
}
