using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Xpo;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using DX.Data;
using DX.Utils;
using DXMVCTestApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
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

		protected virtual T PopulateModel<T>(T model, string values)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));
			if (!string.IsNullOrWhiteSpace(values))
			{
				var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
				valuesDict.AssignToObject(model);
			}
			return model;
		}

		protected string GetFullErrorMessage(ModelStateDictionary modelState)
		{
			var messages = new List<string>();

			foreach (var entry in modelState)
			{
				foreach (var error in entry.Value.Errors)
					messages.Add(error.ErrorMessage);
			}

			return string.Join(" ", messages);
		}

	}

	public class BaseController<TMainStore, TKey, TModel> : BaseController
		where TMainStore : DX.Data.IQueryableDataStore<TKey, TModel>
		where TKey : IEquatable<TKey>
		where TModel : class, new()
	{
		public TMainStore MainStore { get; }
		public virtual string PartialViewName { get; } = "GridViewPartialView";
		public BaseController() : base()
		{
			MainStore = (TMainStore)Activator.CreateInstance(typeof(TMainStore), DataLayer);
		}

		//protected override void OnException(ExceptionContext filterContext)
		//{
		//	var err = filterContext.Exception;
		//	var resp = filterContext.HttpContext.Response;
		//	var req = filterContext.HttpContext.Request;

		//	FluentValidation.ValidationException vEx = err as FluentValidation.ValidationException;

		//	for (int i = 0; i < vEx.Errors.Count(); i++)
		//	{
		//	}

		//	base.OnException(filterContext);
		//}

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

		//[HttpPost, ValidateInput(true)]
		public async virtual Task<ActionResult> Create(TModel item)
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
		public async virtual Task<ActionResult> Delete(TKey oid)
		{
			await MainStore.DeleteAsync(oid);
			return await DXControlPartialView();
		}
	}
}
