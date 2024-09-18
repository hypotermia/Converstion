using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DevExpress.Xpo;
using DXMVCTestApplication.Models;
using DXMVCTestApplication.Models.XPO;


namespace DXMVCTestApplication.Controllers
{
    public class HomeController : BaseController<PatientStore, int, Patient>
    {
		
		public HomeController() : base()
		{
		}

		[HttpPost, ValidateInput(true)]
		public async override Task<ActionResult> AddNew(Patient item)
		{
			return await base.AddNew(item);
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
	}
}