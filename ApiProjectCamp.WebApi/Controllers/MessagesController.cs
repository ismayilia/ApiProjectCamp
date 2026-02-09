using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Dtos.MessageDtos;
using ApiProjectCamp.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class MessagesController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ApiContext _context;

		public MessagesController(ApiContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpGet]
		public IActionResult MessageList()
		{
			var values = _context.Messages.ToList();
			return Ok(_mapper.Map<List<ResultMessageDto>>(values));
		}

		[HttpPost]
		public IActionResult CreateMessage(CreateMessageDto createMessageDto)
		{
			var value = _mapper.Map<Message>(createMessageDto);
			_context.Messages.Add(value);
			_context.SaveChanges();
			return Ok("Added Success!");
		}

		[HttpDelete]
		public IActionResult DeleteMessage(int id)
		{
			var value = _context.Messages.Find(id);
			_context.Messages.Remove(value);
			_context.SaveChanges();
			return Ok("Deleted Success");
		}

		[HttpGet]
		public IActionResult GetMessage (int id)
		{
			var value = _context.Messages.Find(id);
			return Ok(_mapper.Map<GetByIdMessageDto>(value));
		}

		[HttpPut]
		public IActionResult UpdateMessage(UpdateMessageDto updateMessageDto)
		{
			var value = _mapper.Map<Message>(updateMessageDto);
			_context.Messages.Update(value);
			_context.SaveChanges();
			return Ok("Updates Success");
		}

		[HttpGet]
		public IActionResult MessageListByIsReadFalse()
		{
			var value = _context.Messages.Where(x => x.IsRead == false);
			return Ok(value);
		}
	}
}
