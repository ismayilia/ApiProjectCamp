using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Dtos.ImageDtos;
using ApiProjectCamp.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ApiContext _context;

		public ImagesController(ApiContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpGet]
		public IActionResult ImageList()
		{
			var values = _context.Images.ToList();
			return Ok(_mapper.Map<List<ResultImageDto>>(values));
		}

		[HttpPost]
		public IActionResult CreateImage(CreateImageDto createImageDto)
		{
			var value = _mapper.Map<Image>(createImageDto);
			_context.Images.Add(value);
			_context.SaveChanges();
			return Ok("Added Success!");
		}

		[HttpDelete]
		public IActionResult DeleteImage(int id)
		{
			var value = _context.Images.Find(id);
			_context.Images.Remove(value);
			_context.SaveChanges();
			return Ok("Deleted Success");
		}

		[HttpGet]
		public IActionResult GetImage(int id)
		{
			var value = _context.Images.Find(id);
			return Ok(_mapper.Map<GetImageByIdDto>(value));
		}

		[HttpPut]
		public IActionResult UpdateImage(UpdateImageDto updateImageDto)
		{
			var value = _mapper.Map<Image>(updateImageDto);
			_context.Images.Update(value);
			_context.SaveChanges();
			return Ok("Updates Success");
		}
	}
}
