using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using DevExpress.Xpo;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using DX.Utils;
using DXMVCTestApplication.Models;
using DXMVCTestApplication.Models.XPO;
using Newtonsoft.Json;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;


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
	}
}