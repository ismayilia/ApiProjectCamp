using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Dtos.CategoryDtos;
using ApiProjectCamp.WebApi.Dtos.FeatureDtos;
using ApiProjectCamp.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApiContext _context;
		private readonly IMapper _mapper;


		public CategoriesController(ApiContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpGet]
		public IActionResult CategoryList()
		{
			var values = _context.Categories.ToList();
			return Ok(values);
		}

		[HttpPost]
		public IActionResult CreateCategory(CreateCategoryDto createCategoryDto)
		{
			//_context.Categories.Add(category);
			//_context.SaveChanges();
			var value = _mapper.Map<Category>(createCategoryDto);
			_context.Categories.Add(value);
			_context.SaveChanges();
			return Ok("Category create success");
		}

		[HttpDelete]
		public IActionResult DeleteCategory(int id)
		{
			//var value = _context.Categories.FirstOrDefault(m=> m.CategoryId==id);
			var value = _context.Categories.Find(id);
			_context.Categories.Remove(value);
			_context.SaveChanges();
			return Ok("Category");
		}

		[HttpGet]
		public IActionResult GetCategory([FromQuery] int id) //burada query yazsaqda olar yazmasaqda,
			//Route-da "[action]" veya [HttpGet("GetCategory")] veya
			//["hansisaad"] ve ya [HttpGet("[action]")] olduqda [FromQuery] yazmasaqda olar
		{
			var value = _context.Categories.Find(id);
			return Ok(value);
		}

		[HttpPut]
		public IActionResult PutCategory(Category category) 
		{
			_context.Categories.Update(category);
			_context.SaveChanges();
			return Ok("Update success");
		}
	}
}
