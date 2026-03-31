using ApiProjectCamp.WebUI.Dtos.ServiceDtos;
using ApiProjectCamp.WebUI.Dtos.WhyChooseYummyDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ApiProjectCamp.WebUI.Controllers
{
	public class WhyChooseYummyController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public WhyChooseYummyController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		[HttpGet]
		public async Task<IActionResult> WhyChooseYummyList()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7256/api/Services/ServiceList");
			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultWhyChooseYummyDto>>(jsonData);
				return View(values);
			}
			return View();
		}

		[HttpGet]
		public IActionResult CreateWhyChooseYummy()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateWhyChooseYummy(CreateWhyChooseYummyDto createWhyChooseYummyDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(createWhyChooseYummyDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync("https://localhost:7256/api/Services/CreateService", stringContent);
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("WhyChooseYummyList");
			}
			return View();
		}

		//[HttpPost]
		public async Task<IActionResult> DeleteWhyChooseYummy(int id)
		{
			var client = _httpClientFactory.CreateClient();
			await client.DeleteAsync("https://localhost:7256/api/Services/DeleteService?id=" + id);
			return RedirectToAction("WhyChooseYummyList");

		}

		[HttpGet]
		public async Task<IActionResult> UpdateWhyChooseYummy(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7256/api/Services/GetService?id=" + id);
			var jsonData = await responseMessage.Content.ReadAsStringAsync();
			var value = JsonConvert.DeserializeObject<GetWhyChooseYummyByIdDto>(jsonData);
			return View(value);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateWhyChooseYummy(UpdateWhyChooseYummyDto updateWhyChooseYummyDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(updateWhyChooseYummyDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PutAsync("https://localhost:7256/api/Services/PutService", stringContent);

			return RedirectToAction("WhyChooseYummyList");

		}
	}
}
