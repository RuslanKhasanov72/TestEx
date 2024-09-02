namespace TestEx.ApiModels
{
	public class PatientList
	{
		public int Id { get; set; }
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string Address { get; set; }
		public DateTime BirthDate { get; set; }
		public bool Gender { get; set; }
		public int DistrictNumber { get; set; }
	}
}
