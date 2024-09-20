using DevExtreme.AspNet.Mvc;
using DXMVCTestApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DXMVCTestApplication.Controllers
{
	[Route("api/{controller}/{action}")]
	public class PatientsController : BaseApiController<PatientStore, Patient>
	{
		public PatientsController(): base()
		{

		}

		[HttpGet]
		public async override Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
		{
			return await base.Get(loadOptions);
		}
		[HttpPost]
		public async override Task<HttpResponseMessage> InsertItem(FormDataCollection form)
		{
			return await base.InsertItem(form);
		}
		[HttpPut]
		public async override Task<HttpResponseMessage> UpdateItem(FormDataCollection form)
		{
			return await base.UpdateItem(form);
		}
		[HttpDelete]
		public async override Task<HttpResponseMessage> DeleteItem(int key)
		{
			return await base.DeleteItem(key);
		}
	}
}