using ApiProjectCamp.WebApi.Context;
using ApiProjectCamp.WebApi.Dtos.ReservationDtos;
using ApiProjectCamp.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjectCamp.WebApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ReservationsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ApiContext _context;

		public ReservationsController(ApiContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpGet]
		public IActionResult ReservationList()
		{
			var values = _context.Reservations.ToList();
			return Ok(_mapper.Map<List<ResultReservationDto>>(values));
		}

		[HttpPost]
		public IActionResult CreateReservation(CreateReservationDto createReservationDto)
		{
			var value = _mapper.Map<Reservation>(createReservationDto);
			_context.Reservations.Add(value);
			_context.SaveChanges();
			return Ok("Added Success!");
		}

		[HttpDelete]
		public IActionResult DeleteReservation(int id)
		{
			var value = _context.Reservations.Find(id);
			_context.Reservations.Remove(value);
			_context.SaveChanges();
			return Ok("Deleted Success");
		}

		[HttpGet]
		public IActionResult GetReservation(int id)
		{
			var value = _context.Reservations.Find(id);
			return Ok(_mapper.Map<GetReservationByIdDto>(value));
		}

		[HttpPut]
		public IActionResult UpdateReservation(UpdateReservationDto updateReservationDto)
		{
			var value = _mapper.Map<Reservation>(updateReservationDto);
			_context.Reservations.Update(value);
			_context.SaveChanges();
			return Ok("Updates Success");
		}

	}
}
