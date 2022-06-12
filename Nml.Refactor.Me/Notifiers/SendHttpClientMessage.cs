using Nml.Refactor.Me.Dependencies;
using Nml.Refactor.Me.MessageBuilders;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Nml.Refactor.Me.Notifiers
{
    public static class SendHttpClientMessage
	{
        public static async Task SendMessageAsync(string url, NotificationMessage message, IWebhookMessageBuilder messageBuilder, ILogger logger) {
			var serviceEndPoint = new Uri(url);
			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Post, serviceEndPoint);
			request.Content = new StringContent(
				messageBuilder.CreateMessage(message).ToString(),
				Encoding.UTF8,
				"application/json");
			try
			{
				var response = await client.SendAsync(request);
				logger.LogTrace($"Message sent. {response.StatusCode} -> {response.Content}");
			}
			catch (AggregateException e)
			{
				foreach (var exception in e.Flatten().InnerExceptions)
					logger.LogError(exception, $"Failed to send message. {exception.Message}");

				throw;
			}
		}
    }
}
