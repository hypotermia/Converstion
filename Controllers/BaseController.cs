using DevExpress.Xpo;
using DX.Data;
using DX.Utils;
using DXMVCTestApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;

namespace DXMVCTestApplication.Controllers
{
	public interface IXpoController
	{
		IDataLayer DataLayer { get; }
	}
	public class BaseController : Controller, IXpoController
	{
		public BaseController()
		{
			dataLayer = XpoHelper.GetDataLayer();
		}

		IDataLayer dataLayer;
		public IDataLayer DataLayer => dataLayer;

	}

	public class BaseController<TMainStore, TKey, TModel> : BaseController
		where TMainStore: DX.Data.IQueryableDataStore<TKey, TModel>
		where TKey : IEquatable<TKey>
		where TModel : class, new()	
	{
		public TMainStore MainStore { get; }
		public virtual string PartialViewName { get; } = "GridViewPartialView";
		public BaseController() : base()
		{			
			MainStore = (TMainStore)Activator.CreateInstance(typeof(TMainStore), DataLayer);
		}

		public async virtual Task<ActionResult> Index()
		{
			var data = await MainStore.Query().ToListAsync();
			return View(data);
		}

		public async virtual Task<ActionResult> DXControlPartialView()
		{
			var data = await MainStore.Query().ToListAsync();
			return PartialView(PartialViewName, data);
		}


		protected IDataResult<TKey, TModel> HandleValidation(IDataResult<TKey, TModel> dataResult, TModel model)
		{
			if (!dataResult.Success)
			{
				ViewData["EditError"] = string.Join("\\n", dataResult.Exception.Errors);
				ViewData["EditableItem"] = model;
				foreach (var error in dataResult.Exception.Errors)
					ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
			}
			return dataResult;				
		}

		//[HttpPost, ValidateInput(true)]
		public async virtual Task<ActionResult> AddNew(TModel item)
		{
			if (ModelState.IsValid)
			{
				var result = HandleValidation(await MainStore.CreateAsync(item), item);
			}
			else
			{
				ViewData["EditError"] = "Please, correct all errors.";
				ViewData["EditableItem"] = item;
			}
			return await DXControlPartialView();
		}

		//[HttpPost, ValidateInput(true)]
		public async virtual Task<ActionResult> Update(TModel item)
		{
			if (ModelState.IsValid)
			{
				var result = HandleValidation(await MainStore.UpdateAsync(item), item);
			}
			else
			{
				ViewData["EditError"] = "Please, correct all errors.";
				ViewData["EditableItem"] = item;
			}
			return await DXControlPartialView();
		}

		//[HttpPost, ValidateInput(false)]
		public async  virtual Task<ActionResult> Delete(TKey oid)
		{
			await MainStore.DeleteAsync(oid);
			return await DXControlPartialView();
		}



	}
}
