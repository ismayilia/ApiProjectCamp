using ApiProjectCamp.WebUI.Dtos.ContactDtos;
using ApiProjectCamp.WebUI.Dtos.ReservationDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ApiProjectCamp.WebUI.Controllers
{
	public class ContactController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public ContactController(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		[HttpGet]
		public async Task<IActionResult> ContactList()
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7256/api/Contacts/ContactList");
			if (responseMessage.IsSuccessStatusCode)
			{
				var jsonData = await responseMessage.Content.ReadAsStringAsync();
				var values = JsonConvert.DeserializeObject<List<ResultContactDto>>(jsonData);
				return View(values);
			}
			return View();
		}

		[HttpGet]
		public IActionResult CreateContact()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreateContact(CreateContactDto createContactDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(createContactDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PostAsync("https://localhost:7256/api/Contacts/CreateContact", stringContent);
			if (responseMessage.IsSuccessStatusCode)
			{
				return RedirectToAction("ContactList");
			}
			return View();
		}

		//[HttpPost]
		public async Task<IActionResult> DeleteContact(int id)
		{
			var client = _httpClientFactory.CreateClient();
			await client.DeleteAsync("https://localhost:7256/api/Contacts/DeleteContact?id=" + id);
			return RedirectToAction("ContactList");

		}

		[HttpGet]
		public async Task<IActionResult> UpdateContact(int id)
		{
			var client = _httpClientFactory.CreateClient();
			var responseMessage = await client.GetAsync("https://localhost:7256/api/Contacts/GetContact?id=" + id);
			var jsonData = await responseMessage.Content.ReadAsStringAsync();
			var value = JsonConvert.DeserializeObject<GetContactByIdDto>(jsonData);
			return View(value);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateContact(UpdateContactDto updateContactDto)
		{
			var client = _httpClientFactory.CreateClient();
			var jsonData = JsonConvert.SerializeObject(updateContactDto);
			StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
			var responseMessage = await client.PutAsync("https://localhost:7256/api/Contacts/UpdateContact", stringContent);

			return RedirectToAction("ContactList");

		}
	}
}
