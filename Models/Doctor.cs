using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestEx.Models
{
	public class Doctor
	{
		public int Id { get; set; }
		public string FullName { get; set; }

		public int RoomId { get; set; }
		public virtual Room Room { get; set; }

		public int SpecializationId { get; set; }
		public virtual Specialization Specialization { get; set; }

		public int? DistrictId { get; set; } // Nullable, as not all doctors might have a district
		public virtual District District { get; set; }
	}
}
