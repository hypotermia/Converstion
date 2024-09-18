using DXMVCTestApplication.Models.XPO;
using FluentValidation;

namespace DXMVCTestApplication.Models
{
	public class XPAppointmentValidator : AbstractValidator<XPAppointment>
	{
		public XPAppointmentValidator()
		{
			RuleFor(x => x.PatientId).GreaterThan(0);
			RuleFor(x => x.Date).NotEmpty();
			RuleFor(x => x.Duration).GreaterThan(0);
		}
	}
}