using System;
using System.Linq;

namespace DXMVCTestApplication.Models
{
	public class Appointment
	{
		public int Oid { get; set; }
		public int PatientId { get; set; }
		public string PatientName { get; set; }
		public DateTime Date { get; set; }
		public double Duration { get; set; }
		public bool AllDayEvent { get; set; }
		public AppointmentStatus Status { get; set; }
		public string Comment { get; set; }

		public DateTime EndDate
		{
			get => Date.AddMinutes(Duration);
			set => Duration = value.Subtract(Date).TotalMinutes;
		}

		public string Subject => PatientName ?? "Patient Appointment";
	}
}