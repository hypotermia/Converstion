using DXMVCTestApplication.Models.XPO;
using DXMVCTestApplication.Models;
using Mapster;
using Microsoft.Owin;
using Owin;
using System.Threading;

[assembly: OwinStartup(typeof(DXMVCTestApplication.Startup))]

// Files related to ASP.NET Identity duplicate the Microsoft ASP.NET Identity file structure and contain initial Microsoft comments.

namespace DXMVCTestApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			//ConfigureAuth(app);
			//Mapster map configuration
			TypeAdapterConfig<Appointment, XPAppointment>.NewConfig()
				.BeforeMapping((src, dest) => dest.Patient = dest.Session.GetObjectByKey<XPPatient>(src.PatientId));
		}
	}
}