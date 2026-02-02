using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class YummyEventsController : ControllerBase
	{
		private readonly ApiContext _context;

		public YummyEventsController(ApiContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult YummyEventList()
		{
			var values = _context.YummyEvents.ToList();
			return Ok(values);
		}

		[HttpPost]
		public IActionResult CreateYummyEvent(YummyEvent YummyEvent)
		{
			_context.YummyEvents.Add(YummyEvent);
			_context.SaveChanges();
			return Ok("YummyEvent create success");
		}

		[HttpDelete]
		public IActionResult DeleteYummyEvent(int id)
		{
			//var value = _context.YummyEvents.FirstOrDefault(m=> m.YummyEventId==id);
			var value = _context.YummyEvents.Find(id);
			_context.YummyEvents.Remove(value);
			_context.SaveChanges();
			return Ok("YummyEvent is deleted!");
		}

		[HttpGet]
		public IActionResult GetYummyEvent([FromQuery] int id) //burada query yazsaqda olar yazmasaqda,
															 //Route-da "[action]" veya [HttpGet("GetYummyEvent")] veya
															 //["hansisaad"] ve ya [HttpGet("[action]")] olduqda [FromQuery] yazmasaqda olar
		{
			var value = _context.YummyEvents.Find(id);
			return Ok(value);
		}

		[HttpPut]
		public IActionResult PutYummyEvent(YummyEvent YummyEvent)
		{
			_context.YummyEvents.Update(YummyEvent);
			_context.SaveChanges();
			return Ok("Update success");
		}
	}
}
