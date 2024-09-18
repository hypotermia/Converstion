using System;
using System.Linq;

namespace DXMVCTestApplication.Models
{
	public class Patient
	{
		public int Oid { get; set; }
		public string FullName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime Birthday { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string Phone { get; set; }
		//public string Email { get; set; }

		public DateTime LastVisit { get; set; }
		public DateTime NextVisit { get; set; }
	}
}