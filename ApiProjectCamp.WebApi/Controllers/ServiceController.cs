using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ServiceController : ControllerBase
	{
		private readonly ApiContext _context;

		public ServiceController(ApiContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult ServiceList()
		{
			var values = _context.Services.ToList();
			return Ok(values);
		}

		[HttpPost]
		public IActionResult CreateService(Service service)
		{
			_context.Services.Add(service);
			_context.SaveChanges();
			return Ok("Service create success");
		}

		[HttpDelete]
		public IActionResult DeleteService(int id)
		{
			//var value = _context.Services.FirstOrDefault(m=> m.ServiceId==id);
			var value = _context.Services.Find(id);
			_context.Services.Remove(value);
			_context.SaveChanges();
			return Ok("Service deleted!");
		}

		[HttpGet]
		public IActionResult GetService([FromQuery] int id) //burada query yazsaqda olar yazmasaqda,
															 //Route-da "[action]" veya [HttpGet("GetService")] veya
															 //["hansisaad"] ve ya [HttpGet("[action]")] olduqda [FromQuery] yazmasaqda olar
		{
			var value = _context.Services.Find(id);
			return Ok(value);
		}

		[HttpPut]
		public IActionResult PutService(Service service)
		{
			_context.Services.Update(service);
			_context.SaveChanges();
			return Ok("Update success");
		}
	}
}
