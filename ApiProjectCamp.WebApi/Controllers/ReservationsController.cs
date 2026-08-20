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

		[HttpGet]
		public IActionResult GetPendingReservations()
		{
			var value = _context.Reservations.Where(m => m.ReservationStatus == ("Onay Bekliyor")).Count();
			return Ok(value);
		}

		[HttpGet]
		public IActionResult GetApprovedReservations()
		{
			var value = _context.Reservations.Where(m => m.ReservationStatus == ("Onaylandı")).Count();
			return Ok(value);
		}

		[HttpGet]
		public IActionResult GetTotalCustomerCount()
		{
			var value = _context.Reservations.Sum(x => x.CountOfPeople);
			return Ok(value);
		}

		[HttpGet]
		public IActionResult GetTotalReservationCount()
		{
			var value = _context.Reservations.Count();
			return Ok(value);
		}

		[HttpGet]
		public IActionResult GetReservationStats()
		{
			DateTime today = new DateTime(2025, 11, 19);
			DateTime fourMonthsAgo = today.AddMonths(-3); ;

			// 1. SQL tarafında sadece gruplama ve veri çekme
			var rawData = _context.Reservations
				.Where(r => r.ReservationDate >= fourMonthsAgo)
				.GroupBy(r => new { r.ReservationDate.Year, r.ReservationDate.Month })
				.Select(g => new
				{
					g.Key.Year,
					g.Key.Month,
					Approved = g.Count(x => x.ReservationStatus == "Onaylandi"),
					Pending = g.Count(x => x.ReservationStatus == "Onay Bekliyor"),
					Canceled = g.Count(x => x.ReservationStatus == "Iptal Edildi")
				})
				.OrderBy(x => x.Year).ThenBy(x => x.Month)
				.ToList(); // Burada SQL biter, veriler RAM’e alınır

			// 2. Bellekte DTO'ya mapleme + tarih formatlama
			var result = rawData.Select(x => new ReservationChartDto
			{
				Month = new DateTime(x.Year, x.Month, 1).ToString("MMM yyyy"),
				Approved = x.Approved,
				Pending = x.Pending,
				Canceled = x.Canceled
			}).ToList();

			return Ok(result);
		}

	}
}
