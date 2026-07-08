using ApiProjectCamp.WebUI.Dtos.MessageDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static ApiProjectCamp.WebUI.Controllers.AIController;

namespace ApiProjectCamp.WebUI.Controllers
{
	public class MessageController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public MessageController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		[HttpGet]
		public async Task<IActionResult> MessageList()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7256/api/Messages/MessageList");
			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultMessageDto>>(jsonData);
				return View(values);
			}
			return View();
		}

		[HttpGet]
		public IActionResult CreateMessage()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateMessage(CreateMessageDto createMessageDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(createMessageDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync("https://localhost:7256/api/Messages/CreateMessage", stringContent);
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("MessageList");
			}
			return View();
		}

		//[HttpPost]
		public async Task<IActionResult> DeleteMessage(int id)
		{
			var client = _httpClientFactory.CreateClient();
			await client.DeleteAsync("https://localhost:7256/api/Messages/DeleteMessage?id=" + id);
			return RedirectToAction("MessageList");

		}

		[HttpGet]
		public async Task<IActionResult> UpdateMessage(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7256/api/Messages/GetMessage?id=" + id);
			var jsonData = await responseMessage.Content.ReadAsStringAsync();
			var value = JsonConvert.DeserializeObject<GetMessageByIdDto>(jsonData);
			return View(value);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateMessage(UpdateMessageDto updateMessageDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(updateMessageDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PutAsync("https://localhost:7256/api/Messages/UpdateMessage", stringContent);

			return RedirectToAction("MessageList");

		}

		[HttpGet]
		public async Task<IActionResult> AnswerMessageWithOpenAi(int id, string prompt)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7256/api/Messages/GetMessage?id=" + id);
			var jsonData = await responseMessage.Content.ReadAsStringAsync();
			var value = JsonConvert.DeserializeObject<GetMessageByIdDto>(jsonData);
			prompt = value.MessageDetails;

			var apiKey = "";

			using var client2 = new HttpClient();
			client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

			var requestData = new
			{
				model = "gpt-3.5-turbo",
				messages = new[]
				{
					new
					{
						role="system",
						content="You are a customer service AI assistant for a restaurant. You respond to customer messages in a detailed, polite, professional, and customer-oriented manner. Always prioritize customer satisfaction, empathy, and clear communication. Generate positive, helpful, and logical responses while maintaining a warm and welcoming tone."
					},
					new
					{
						role="user",
						content= prompt
					}
				},
				temperature = 0.5
			};

			var response = await client2.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestData);

			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
				var content = result.choices[0].message.content;
				ViewBag.answerAI = content;
			}
			else
			{
				ViewBag.answerAI = "An error occurred: " + response.StatusCode;
			}

			return View(value);
		}

		public PartialViewResult SendMessage()
		{
			return PartialView();
		}

		[HttpPost]
		public async Task<IActionResult> SendMessage(CreateMessageDto createMessageDto)
		{
			var client = new HttpClient();
			var apiKey = "";
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
			try
			{
				var translateRequestBody = new
				{
					input = createMessageDto.MessageDetails
				};
				var translateJson = System.Text.Json.JsonSerializer.Serialize(translateRequestBody);
				var translateContent = new StringContent(translateJson, Encoding.UTF8, "application/json");
				var translateResponse = await client.PostAsync("https://api-inference.huggingface.co/models/Helsinki-NLP/opus-mt-tr-en", translateContent);
				var translateResponseString = await translateResponse.Content.ReadAsStringAsync();

				string englishText = createMessageDto.MessageDetails;
				if (translateResponseString.TrimStart().StartsWith("["))
				{
					var translateDoc = JsonDocument.Parse(translateResponseString);
					englishText = translateDoc.RootElement[0].GetProperty("translation_text").GetString();
					//ViewBag.v = englishText;
				}

				var toxicRequestBody = new
				{
					inputs = englishText
				};

				var toxisJson = System.Text.Json.JsonSerializer.Serialize(toxicRequestBody);
				var toxicContent = new StringContent(toxisJson, Encoding.UTF8, "application/json");
				var toxicResponse = await client.PostAsync("https://api-inference.huggingface.co/models/unitary/toxic-bert", toxicContent);
				var toxicResponseString = await toxicResponse.Content.ReadAsStringAsync();

				if (toxicResponseString.TrimStart().StartsWith("["))
				{
					var toxicDoc = JsonDocument.Parse(toxicResponseString);
					foreach (var item in toxicDoc.RootElement[0].EnumerateArray())
					{
						string label = item.GetProperty("label").GetString();
						// 0.01-0.99 arasi deyishir toxic-lik derecesi
						double score = item.GetProperty("score").GetDouble();

						if (score > 0.5)
						{
							createMessageDto.Status = "Toxic Message";
							break;
						}

					}
				}

				if (string.IsNullOrEmpty(createMessageDto.Status))
				{
					createMessageDto.Status = "Message received";
				}
			}
			catch
			{

				createMessageDto.Status = "Pending Approval";
			}


			var client2 = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(createMessageDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client2.PostAsync("https://localhost:7256/api/Messages/CreateMessage", stringContent);
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("MessageList");
			}
			return View();
		}
	}
}
