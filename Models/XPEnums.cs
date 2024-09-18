using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXMVCTestApplication.Models
{
	public enum AppointmentStatus { Open, Completed, Failed, Canceled }
	public enum PaymentMethod
	{
		Cash,
		Card
	}
	public enum PaymentStatus
	{
		Unpaid,
		[System.ComponentModel.DataAnnotations.Display(Name = "Paid In Full")]
		PaidInFull,
		[System.ComponentModel.DataAnnotations.Display(Name = "Refund In Full")]
		RefundInFull
	}
	[Flags]
	public enum ProcedureGroup { Diagnosis = 0, Restoration = 1, RootCanal = 2, Hygiene = 4, Whitening = 8, Prosthetics = 16, Implantation = 32, Orthodontics = 64, Surgery = 128 }
	public enum ProcedureType { General, Tooth }
}