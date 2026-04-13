using ApiProjectCamp.WebUI.Dtos.YummyEventDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ApiProjectCamp.WebUI.Controllers
{
	public class YummyEventController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public YummyEventController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		[HttpGet]
		public async Task<IActionResult> YummyEventList()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7256/api/YummyEvents/YummyEventList");
			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultYummyEventDto>>(jsonData);
				return View(values);
			}
			return View();
		}

		[HttpGet]
		public IActionResult CreateYummyEvent()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateYummyEvent(CreateYummyEventDto createYummyEventDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(createYummyEventDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync("https://localhost:7256/api/YummyEvents/CreateYummyEvent", stringContent);
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("YummyEventList");
			}
			return View();
		}

		//[HttpPost]
		public async Task<IActionResult> DeleteYummyEvent(int id)
		{
			var client = _httpClientFactory.CreateClient();
			await client.DeleteAsync("https://localhost:7256/api/YummyEvents/DeleteYummyEvent?id=" + id);
			return RedirectToAction("YummyEventList");

		}

		[HttpGet]
		public async Task<IActionResult> UpdateYummyEvent(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7256/api/YummyEvents/GetYummyEvent?id=" + id);
			var jsonData = await responseMessage.Content.ReadAsStringAsync();
			var value = JsonConvert.DeserializeObject<GetYummyEventByIdDto>(jsonData);
			return View(value);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateYummyEvent(UpdateYummyEventDto updateYummyEventDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(updateYummyEventDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PutAsync("https://localhost:7256/api/YummyEvents/PutYummyEvent", stringContent);

			return RedirectToAction("YummyEventList");

		}
	}
}
