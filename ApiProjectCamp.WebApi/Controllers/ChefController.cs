using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ChefController : ControllerBase
	{
		private readonly ApiContext _context;

		public ChefController(ApiContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult ChefList()
		{
			var values = _context.Chefs.ToList();
			return Ok(values);
		}

		[HttpPost]
		public IActionResult CreateChef(Chef chef)
		{
			_context.Chefs.Add(chef);
			_context.SaveChanges();
			return Ok("Chef add success!");
		}

		[HttpDelete]
		public IActionResult DeleteChef (int id)
		{
			var value = _context.Chefs.Find(id);
			_context.Remove(value);
			_context.SaveChanges();
			return Ok("Delete Chef success");
		}

		[HttpGet("GetChef")]
		public IActionResult GetChef(int id)
		{
			//var value = _context.Chefs.Find(id);
			//return Ok(value);
			return Ok(_context.Chefs.Find(id));
		}

		[HttpPut]
		public IActionResult UpdateChef(Chef chef)
		{
			_context.Chefs.Update(chef);
			_context.SaveChanges();
			return Ok("Update is success!");
		}
	}
}
