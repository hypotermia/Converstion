using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using DXMVCTestApplication.Models;
using Newtonsoft.Json;



namespace DXMVCTestApplication.Controllers
{
    public class HomeController : BaseController<PatientStore, int, Patient>
    {
		
		public HomeController() : base()
		{
		}

		[HttpPost, ValidateInput(true)]
		public async override Task<ActionResult> Create(Patient item)
		{
			return await base.Create(item);
		}

		[HttpPost, ValidateInput(true)]
		public async override Task<ActionResult> Update(Patient item)
		{
			return await base.Update(item);
		}

		[HttpPost, ValidateInput(false)]
		public async override Task<ActionResult> Delete(int oid)
		{
			return await base.Delete(oid);
		}

		// DevExtreme upgrade

		[HttpGet]
		public async override Task<ActionResult> Get(DataSourceLoadOptions loadOptions)
		{
			return await base.Get(loadOptions);
		}
		[HttpPost]
		public async override Task<ActionResult> InsertItem(string values)
		{
			return await base.InsertItem(values);
		}
		[HttpPut]
		public async override Task<ActionResult> UpdateItem(int key, string values)
		{
			return await base.UpdateItem(key, values);
		}
		[HttpDelete]
		public async override Task DeleteItem(int key)
		{
			await base.DeleteItem(key);
		}
	}
}