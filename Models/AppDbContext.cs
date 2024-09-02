using Microsoft.EntityFrameworkCore;

namespace TestEx.Models
{
	public class AppDbContext:DbContext
	{
		public DbSet<District> Districts { get; set; }
		public DbSet<Specialization> Specializations { get; set; }
		public DbSet<Room> Rooms { get; set; }
		public DbSet<Patient> Patients { get; set; }
		public DbSet<Doctor> Doctors { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options)
			 : base(options)
		{ }
	}
}
