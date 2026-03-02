using ApiProjectCamp.WebUI.Dtos.FeatureDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ApiProjectCamp.WebUI.Controllers
{
	public class FeatureController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public FeatureController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		[HttpGet]
		public async Task<IActionResult> FeatureList()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7256/api/Features/FeatureList");
			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultFeatureDto>>(jsonData);
				return View(values);
			}
			return View();
		}

		[HttpGet]
		public IActionResult CreateFeature()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateFeature(CreateFeatureDto createFeatureDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(createFeatureDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync("https://localhost:7256/api/Features/CreateFeature", stringContent);
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("FeatureList");
			}
			return View();
		}

		//[HttpPost]
		public async Task<IActionResult> DeleteFeature(int id)
		{
			var client = _httpClientFactory.CreateClient();
			await client.DeleteAsync("https://localhost:7256/api/Features/DeleteFeature?id=" + id);
			return RedirectToAction("FeatureList");

		}

		[HttpGet]
		public async Task<IActionResult> UpdateFeature(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7256/api/Features/GetFeature?id=" + id);
			var jsonData = await responseMessage.Content.ReadAsStringAsync();
			var value = JsonConvert.DeserializeObject<GetFeatureByIdDto>(jsonData);
			return View(value);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateFeature(UpdateFeatureDto updateFeatureDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(updateFeatureDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PutAsync("https://localhost:7256/api/Features/PutFeature", stringContent);

			return RedirectToAction("FeatureList");

		}
	}
}
