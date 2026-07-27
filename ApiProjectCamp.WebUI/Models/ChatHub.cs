using Microsoft.AspNetCore.SignalR;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ApiProjectCamp.WebUI.Models
{
	public class ChatHub : Hub
	{
		private const string apikey = "";
		private const string modelGpt = "gpt-4o-mini";
		private readonly IHttpClientFactory _httpClientFactory;

		public ChatHub(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		private static readonly Dictionary<string, List<Dictionary<string, string>>> _history = new();

		public override Task OnConnectedAsync()
		{
			_history[Context.ConnectionId] = new List<Dictionary<string, string>>
			{
				new Dictionary<string, string>
				{
					["role"] = "system",["content"] ="You are a helpful assistant. Keep answers concise."
				}
			};

			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			_history.Remove(Context.ConnectionId);
			return base.OnDisconnectedAsync(exception);
		}

		public async Task SendMessage(string userMessage)
		{
			await Clients.Caller.SendAsync("ReceiveUserEcho", userMessage);
			var history = _history[Context.ConnectionId];
			history.Add(new() { ["role"] = "user", ["content"] = userMessage });
			await StreamOpenAI(history, Context.ConnectionAborted);
		}

		public async Task StreamOpenAI(List<Dictionary<string, string>> history, CancellationToken cancellationToken)
		{
			var client = _httpClientFactory.CreateClient("openai");
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apikey);
			var payload = new
			{
				model = modelGpt,
				messages = history,
				stream = true,
				temperature = 0.2
			};

			using var req = new HttpRequestMessage(HttpMethod.Post, "v1/chat/completions");
			req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "applicaton/json");

			using var resp = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
			resp.EnsureSuccessStatusCode();

			using var stream = await resp.Content.ReadAsStreamAsync(cancellationToken);
			using var reader = new StreamReader(stream);
		}
	}
}
