using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Dtos.AboutDtos;
using ApiProjectCamp.WebApi.Dtos.AboutDtos;
using ApiProjectCamp.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AboutsController : ControllerBase
	{
		private readonly ApiContext _context;
		private readonly IMapper _mapper;


		public AboutsController(ApiContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpGet]
		public IActionResult AboutList()
		{
			var values = _context.Abouts.ToList();
			return Ok(values);
		}

		[HttpPost]
		public IActionResult CreateAbout(CreateAboutDto createAboutDto)
		{
			//_context.Abouts.Add(About);
			//_context.SaveChanges();
			var value = _mapper.Map<About>(createAboutDto);
			_context.Abouts.Add(value);
			_context.SaveChanges();
			return Ok("About create success");
		}

		[HttpDelete]
		public IActionResult DeleteAbout(int id)
		{
			//var value = _context.Abouts.FirstOrDefault(m=> m.AboutId==id);
			var value = _context.Abouts.Find(id);
			_context.Abouts.Remove(value);
			_context.SaveChanges();
			return Ok("About is delete");
		}

		[HttpGet]
		public IActionResult GetAbout([FromQuery] int id) //burada query yazsaqda olar yazmasaqda,
															 //Route-da "[action]" veya [HttpGet("GetAbout")] veya
															 //["hansisaad"] ve ya [HttpGet("[action]")] olduqda [FromQuery] yazmasaqda olar
		{
			var value = _context.Abouts.Find(id);
			return Ok(value);
		}

		[HttpPut]
		public IActionResult PutAbout(UpdateAboutDto updateAboutDto)
		{
			var value = _mapper.Map<About>(updateAboutDto);

			_context.Abouts.Update(value);
			_context.SaveChanges();
			return Ok("Update success");
		}
	}
}
