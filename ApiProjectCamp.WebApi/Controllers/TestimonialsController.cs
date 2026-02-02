using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class TestimonialsController : ControllerBase
	{
		private readonly ApiContext _context;

		public TestimonialsController(ApiContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IActionResult TestimonialList()
		{
			var values = _context.Testimonials.ToList();
			return Ok(values);
		}

		[HttpPost]
		public IActionResult CreateTestimonial(Testimonial Testimonial)
		{
			_context.Testimonials.Add(Testimonial);
			_context.SaveChanges();
			return Ok("Testimonial create success");
		}

		[HttpDelete]
		public IActionResult DeleteTestimonial(int id)
		{
			//var value = _context.Testimonials.FirstOrDefault(m=> m.TestimonialId==id);
			var value = _context.Testimonials.Find(id);
			_context.Testimonials.Remove(value);
			_context.SaveChanges();
			return Ok("Testimonial deleted!");
		}

		[HttpGet]
		public IActionResult GetTestimonial([FromQuery] int id) //burada query yazsaqda olar yazmasaqda,
															//Route-da "[action]" veya [HttpGet("GetTestimonial")] veya
															//["hansisaad"] ve ya [HttpGet("[action]")] olduqda [FromQuery] yazmasaqda olar
		{
			var value = _context.Testimonials.Find(id);
			return Ok(value);
		}

		[HttpPut]
		public IActionResult PutTestimonial(Testimonial Testimonial)
		{
			_context.Testimonials.Update(Testimonial);
			_context.SaveChanges();
			return Ok("Update success");
		}
	}
}
