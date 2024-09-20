using DXMVCTestApplication.Models.XPO;
using FluentValidation;
using System;

namespace DXMVCTestApplication.Models
{
	public class XPPatientValidator : AbstractValidator<XPPatient>
	{
		public XPPatientValidator()
		{
			RuleFor(x => x.FirstName).NotEmpty();
			RuleFor(x => x.LastName).NotEmpty();
			RuleFor(x => x.Birthday).NotEmpty().LessThan(DateTime.Now);
			RuleFor(x => x.Email).EmailAddress();
			RuleFor(x => x.Address).NotEmpty();
			//RuleFor(x => x.Zip).NotEmpty();
			//RuleFor(x => x.Phone).NotEmpty();
		}
	}
}