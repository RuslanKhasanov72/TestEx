using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestEx.ApiModels;
using TestEx.Models;

namespace TestEx.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DoctorsController : ControllerBase
	{
		private readonly AppDbContext _appDbContext;

		public DoctorsController(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}
		[HttpPost]
		[Route("Add")]
		public async Task<ActionResult> CreateDoctor(DoctorEdit editdoctor)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (!_appDbContext.Districts.Any(d => d.Id == editdoctor.RoomId))
				return BadRequest("Кабинет не найден");

			if (!_appDbContext.Districts.Any(d => d.Id == editdoctor.SpecializationId))
				return BadRequest("Специализация не найден");

			var district = editdoctor.DistrictId.HasValue ? await _appDbContext.Districts.FindAsync(editdoctor.DistrictId.Value) : null;
			if (editdoctor.DistrictId.HasValue && district == null)
				return BadRequest("Район не найден");

			var doctor = new Doctor
			{
				FullName = editdoctor.FullName,
				RoomId = editdoctor.RoomId,
				SpecializationId = editdoctor.SpecializationId,
				DistrictId = editdoctor.DistrictId
			};

			_appDbContext.Doctors.Add(doctor);
			await _appDbContext.SaveChangesAsync();

			return Ok(doctor);
		}
		[HttpPut("Update{id}")]
		public async Task<ActionResult> UpdateDoctor(int id, DoctorEdit editdoctor)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var doctor = await _appDbContext.Doctors.FindAsync(id);
			if (doctor == null)
				return NotFound();

			if (!_appDbContext.Districts.Any(d => d.Id == editdoctor.RoomId))
				return BadRequest("Кабинет не найден");

			if (!_appDbContext.Districts.Any(d => d.Id == editdoctor.SpecializationId))
				return BadRequest("Специализация не найден");

			var district = editdoctor.DistrictId.HasValue ? await _appDbContext.Districts.FindAsync(editdoctor.DistrictId.Value) : null;
			if (editdoctor.DistrictId.HasValue && district == null)
				return BadRequest("Район не найден");

			doctor.FullName = editdoctor.FullName;
			doctor.RoomId = editdoctor.RoomId;
			doctor.SpecializationId = editdoctor.SpecializationId;
			doctor.DistrictId = editdoctor.DistrictId;

			await _appDbContext.SaveChangesAsync();

			return Ok();
		}
		[HttpDelete("Delete{id}")]
		public async Task<ActionResult> DeleteDoctor(int id)
		{
			var doctor = await _appDbContext.Doctors.FindAsync(id);
			if (doctor == null)
				return NotFound();

			_appDbContext.Doctors.Remove(doctor);
			await _appDbContext.SaveChangesAsync();

			return Ok();
		}
		[HttpGet]
		[Route("GetAll")]
		public async Task<ActionResult> GetDoctors(string sortBy = "Id", bool ascending = true, int page = 1, int pageSize = 10)
		{
			var query = _appDbContext.Doctors
				.Include(d => d.Room)
				.Include(d => d.Specialization)
				.Include(d => d.District)
				.AsQueryable();

			
			switch (sortBy.ToLower())
			{
				case "fullname":
					query = ascending ? query.OrderBy(d => d.FullName) : query.OrderByDescending(d => d.FullName);
					break;
				case "room":
					query = ascending ? query.OrderBy(d => d.Room.Number) : query.OrderByDescending(d => d.Room.Number);
					break;
				case "specialization":
					query = ascending ? query.OrderBy(d => d.Specialization.Title) : query.OrderByDescending(d => d.Specialization.Title);
					break;
				case "district":
					query = ascending ? query.OrderBy(d => d.District.Number) : query.OrderByDescending(d => d.District.Number);
					break;
				default:
					query = ascending ? query.OrderBy(d => d.Id) : query.OrderByDescending(d => d.Id);
					break;
			}

			
			var doctors = await query
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			var result = doctors.Select(d => new DoctorList
			{
				Id = d.Id,
				FullName = d.FullName,
				RoomNumber = d.Room.Number,
				SpecializationTitle = d.Specialization.Title,
				DistrictNumber = d.District?.Number
			});

			return Ok(result);
		}
		[HttpGet]
		[Route("GetBy{id:int}")]
		public async Task<ActionResult> GetDoctor(int id)
		{
			var doctor = await _appDbContext.Doctors
				.Include(d => d.Room)
				.Include(d => d.Specialization)
				.Include(d => d.District)
				.SingleOrDefaultAsync(d => d.Id == id);

			if (doctor == null)
				return NotFound();

			var doctorDto = new DoctorEdit
			{
				FullName = doctor.FullName,
				RoomId = doctor.RoomId,
				SpecializationId = doctor.SpecializationId,
				DistrictId = doctor.DistrictId
			};

			return Ok(doctorDto);
		}
	}
}
