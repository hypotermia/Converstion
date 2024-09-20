using FluentValidation;
using System;

namespace DXMVCTestApplication.Models
{
	public class PatientValidator : AbstractValidator<Patient>
	{
		public PatientValidator()
		{
			//nameof(XPPatient.Oid)
			RuleFor(x => x.FirstName).NotEmpty();
			RuleFor(x => x.LastName).NotEmpty();
			RuleFor(x => x.Birthday).NotEmpty().LessThan(DateTime.Now);
			RuleFor(x => x.Email).EmailAddress().NotEmpty();
			//RuleFor(x => x.Address).NotEmpty();
			//RuleFor(x => x.Zip).NotEmpty();
			//RuleFor(x => x.Phone).NotEmpty();
		}
	}
}