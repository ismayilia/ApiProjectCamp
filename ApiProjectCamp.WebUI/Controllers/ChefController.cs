using ApiProjectCamp.WebUI.Dtos.ChefDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ApiProjectCamp.WebUI.Controllers
{
	public class ChefController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public ChefController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		[HttpGet]
		public async Task<IActionResult> ChefList()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7256/api/Chefs/ChefList");
			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultChefDto>>(jsonData);
				return View(values);
			}
			return View();
		}

		[HttpGet]
		public IActionResult CreateChef()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateChef(CreateChefDto createChefDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(createChefDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync("https://localhost:7256/api/Chefs/CreateChef", stringContent);
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("ChefList");
			}
			return View();
		}

		//[HttpPost]
		public async Task<IActionResult> DeleteChef(int id)
		{
			var client = _httpClientFactory.CreateClient();
			await client.DeleteAsync("https://localhost:7256/api/Chefs/DeleteChef?id=" + id);
			return RedirectToAction("ChefList");

		}

		[HttpGet]
		public async Task<IActionResult> UpdateChef(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7256/api/Chefs/GetChef?id=" + id);
			var jsonData = await responseMessage.Content.ReadAsStringAsync();
			var value = JsonConvert.DeserializeObject<GetChefByIdDto>(jsonData);
			return View(value);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateChef(UpdateChefDto updateChefDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(updateChefDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PutAsync("https://localhost:7256/api/Chefs/UpdateChef", stringContent);

			return RedirectToAction("ChefList");

		}
	}
}
