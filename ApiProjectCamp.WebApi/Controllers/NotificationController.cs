using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Dtos.NotificationDtos;
using ApiProjectCamp.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProjectCamp.WebApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class NotificationController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ApiContext _context;

		public NotificationController(IMapper mapper, ApiContext context)
		{
			_mapper = mapper;
			_context = context;
		}

		[HttpGet]
		public IActionResult NotificationList()
		{
			var values = _context.Notifications.ToList();
			return Ok(_mapper.Map<List<ResultNotificationDto>>(values));
		}

		[HttpPost]
		public IActionResult CreateNotification(CreateNotificationDto createNotificationDto)
		{
			var value = _mapper.Map<Notification>(createNotificationDto);
			_context.Notifications.Add(value);
			_context.SaveChanges();
			return Ok("Added Success!");
		}

		[HttpDelete]
		public IActionResult DeleteNotification(int id)
		{
			var value = _context.Notifications.Find(id);
			_context.Notifications.Remove(value);
			_context.SaveChanges();
			return Ok("Removed Success!");
		}

		[HttpGet]
		public IActionResult GetNotification(int id)
		{
			var value = _context.Notifications.Find(id);
			return Ok(_mapper.Map<GetNotificationByIdDto>(value));
		}

		[HttpPut]
		public IActionResult UpdateNotification(UpdateNotificationDto updateNotificationDto)
		{
			var data = _context.Notifications.AsNoTracking()
										.FirstOrDefault(m => m.NotificationId == updateNotificationDto.NotificationId);
			if (data is null)
			{
				return NotFound();
			}
			var value = _mapper.Map<Notification>(updateNotificationDto);
			_context.Notifications.Update(value);
			_context.SaveChanges();
			return Ok("Update Success!");
		}
	}
}
