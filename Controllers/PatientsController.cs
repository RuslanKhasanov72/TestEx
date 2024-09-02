using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestEx.ApiModels;
using TestEx.Models;

namespace TestEx.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PatientsController : ControllerBase
	{
		private readonly AppDbContext _appDbContext;

		public PatientsController(AppDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}
		[HttpPost]
		[Route("Add")]
		public async Task<ActionResult<Patient>> AddPatient(PatientEdit addpatient)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			if(!_appDbContext.Districts.Any(d=>d.Id==addpatient.DistrictId))
				return BadRequest("Район не найден");
			var patient = new Patient
			{
				LastName = addpatient.LastName,
				FirstName = addpatient.FirstName,
				MiddleName = addpatient.MiddleName,
				Address = addpatient.Address,
				BirthDate = addpatient.BirthDate,
				Gender = addpatient.Gender,
				DistrictId = addpatient.DistrictId
			};

			_appDbContext.Patients.Add(patient);
			await _appDbContext.SaveChangesAsync();

			return Ok(patient);
		}
		[HttpPut("Update{id}")]
		public async Task<ActionResult<Patient>> UpdatePatient(int id, PatientEdit editpatient)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var patient = await _appDbContext.Patients.FindAsync(id);
			if (patient == null)
				return NotFound();

			if (!_appDbContext.Districts.Any(d => d.Id == editpatient.DistrictId))
				return BadRequest("Район не найден");

			patient.LastName = editpatient.LastName;
			patient.FirstName = editpatient.FirstName;
			patient.MiddleName = editpatient.MiddleName;
			patient.Address = editpatient.Address;
			patient.BirthDate = editpatient.BirthDate;
			patient.Gender = editpatient.Gender;
			patient.DistrictId = editpatient.DistrictId;

			await _appDbContext.SaveChangesAsync();

			return Ok();
		}

		[HttpDelete("Delete{id}")]
		public async Task<ActionResult> DeletePatient(int id)
		{
			var patient = await _appDbContext.Patients.FindAsync(id);
			if (patient == null)
				return NotFound();

			_appDbContext.Patients.Remove(patient);
			await _appDbContext.SaveChangesAsync();

			return Ok();
		}

		[HttpGet]
		[Route("GetAll")]
		public async Task<ActionResult> GetPatients(string sortBy = "Id", bool ascending = true, int page = 1, int pageSize = 10)
		{
			var query = _appDbContext.Patients.Include(p => p.District).AsQueryable();

			
			switch (sortBy.ToLower())
			{
				case "lastname":
					query = ascending ? query.OrderBy(p => p.LastName) : query.OrderByDescending(p => p.LastName);
					break;
				case "firstname":
					query = ascending ? query.OrderBy(p => p.LastName) : query.OrderByDescending(p => p.FirstName);
					break;
				case "birthdate":
					query = ascending ? query.OrderBy(p => p.LastName) : query.OrderByDescending(p => p.BirthDate);
					break;
				case "gender":
					query = ascending ? query.OrderBy(p => p.LastName) : query.OrderByDescending(p => p.Gender);
					break;
				case "address":
					query = ascending ? query.OrderBy(p => p.LastName) : query.OrderByDescending(p => p.Address);
					break;
				case "district":
					query = ascending ? query.OrderBy(p => p.LastName) : query.OrderByDescending(p => p.District);
					break;
				default:
					query = ascending ? query.OrderBy(p => p.Id) : query.OrderByDescending(p => p.Id);
					break;
			}

			
			var patients = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

			var result = patients.Select(p => new PatientList
			{
				Id = p.Id,
				LastName = p.LastName,
				FirstName = p.FirstName,
				MiddleName = p.MiddleName,
				Address = p.Address,
				BirthDate = p.BirthDate,
				Gender = p.Gender,
				DistrictNumber = p.District.Number
			});

			return Ok(result);
		}

		[HttpGet]
		[Route("GetBy{id:int}")]
		public async Task<ActionResult> GetPatient(int id)
		{
			var patient = await _appDbContext.Patients.FindAsync(id);
			if (patient == null)
				return NotFound();

			var patientDto = new PatientEdit
			{
				LastName = patient.LastName,
				FirstName = patient.FirstName,
				MiddleName = patient.MiddleName,
				Address = patient.Address,
				BirthDate = patient.BirthDate,
				Gender = patient.Gender,
				DistrictId = patient.DistrictId
			};

			return Ok(patientDto);
		}
	}
}
