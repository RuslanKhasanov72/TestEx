namespace TestEx.ApiModels
{
	public class PatientEdit
	{
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string Address { get; set; }
		public DateTime BirthDate { get; set; }
		public bool Gender { get; set; }
		public int DistrictId { get; set; }
	}
}
