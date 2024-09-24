using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Xpo;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using DX.Data;
using DX.Utils;
using DXMVCTestApplication.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

	public static class ControllerExtensions
	{
		public static string GetFullErrorMessage(this Controller controller, ModelStateDictionary modelState)
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

		protected ActionResult SendResponse(object obj, HttpStatusCode statusCode = HttpStatusCode.OK)
		{
			Response.StatusCode = (int)statusCode;
			return Content(JsonConvert.SerializeObject(obj), "application/json");
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

		protected IDataResult<TKey, TModel> HandleValidation(IDataResult<TKey, TModel> dataResult)
		{
			if (!ModelState.IsValid || !dataResult.Success)
			{
				ViewData["EditError"] = string.Join("\\n", dataResult.Exception.Errors);
				ViewData["EditableItem"] = dataResult.Data.LastOrDefault();
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
            var result = HandleValidation(await MainStore.CreateAsync(item));
			return await DXControlPartialView();
		}
		//[HttpPost, ValidateInput(true)]
		public async virtual Task<ActionResult> Update(TModel item)
		{
			var result = HandleValidation(await MainStore.UpdateAsync(item));
			return await DXControlPartialView();
		}

		//[HttpPost, ValidateInput(false)]
		public async virtual Task<ActionResult> Delete(TKey oid)
		{
			await MainStore.DeleteAsync(oid);
			return await DXControlPartialView();
		}

		//DevExtreme upgrades
		//[HttpGet]
		public async virtual Task<ActionResult> Get(DataSourceLoadOptions loadOptions)
		{
			return SendResponse(await DataSourceLoader.LoadAsync(MainStore.Query(), loadOptions));
		}

		//[HttpPost]
		public async virtual Task<ActionResult> InsertItem(string values)
		{
			var item = PopulateModel(new TModel(), values);
			var result = HandleValidation(await MainStore.CreateAsync(item));
			if (!result.Success)
			{				
				var err = string.Join("\r\n", result.Exception.Errors.Select(e => e.ErrorMessage));								
				return SendResponse(err, HttpStatusCode.BadRequest);
			}
			TKey key = MainStore.ModelKey(item);
			return SendResponse(new { key });
		}

		//[HttpPut]
		public async virtual Task<ActionResult> UpdateItem(TKey key, string values)
		{
			var item = PopulateModel(MainStore.GetByKey(key), values);
			var result = HandleValidation(await MainStore.UpdateAsync(item));
			if (!result.Success)
			{
                var err = string.Join("\r\n", result.Exception.Errors.Select(e => e.ErrorMessage));
                return SendResponse(err, HttpStatusCode.BadRequest);
			}
			return new EmptyResult();
		}
		
		//[HttpDelete]
		public async virtual Task DeleteItem(TKey key)
		{
			var item = MainStore.GetByKey(key);
			var result = await MainStore.DeleteAsync(item);
		}
	}
}
