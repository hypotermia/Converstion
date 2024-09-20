using DevExpress.ExpressApp.Security;
using DevExpress.Xpo;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using DX.Data;
using DX.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;

namespace DXMVCTestApplication.Controllers
{
	public class BaseApiController : ApiController, IXpoController
	{
		public IDataLayer DataLayer { get; }

		public BaseApiController()
		{
			DataLayer = XpoHelper.GetDataLayer();
		}

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
		protected string GetFullErrorMessage(System.Web.Http.ModelBinding.ModelStateDictionary modelState)
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
	public abstract class BaseApiController<TMainStore, TKey, TModel> : BaseApiController
		where TMainStore : DX.Data.IQueryableDataStore<TKey, TModel>
		where TKey : IEquatable<TKey>
		where TModel : class, new()
		{
		public TMainStore MainStore { get; }		
		public BaseApiController() : base()
		{
			MainStore = (TMainStore)Activator.CreateInstance(typeof(TMainStore), DataLayer);
		}

		protected IDataResult<TKey, TModel> HandleValidation(IDataResult<TKey, TModel> dataResult, TModel model)
		{
			if (!dataResult.Success)
			{
				foreach (var error in dataResult.Exception.Errors)
					ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
			}
			return dataResult;
		}
		
		public async virtual Task<HttpResponseMessage> Get(DataSourceLoadOptions loadOptions)
		{
			var data = await DataSourceLoader.LoadAsync(MainStore.Query(), loadOptions);
			return Request.CreateResponse(data);
		}
		
		public async virtual Task<HttpResponseMessage> InsertItem(FormDataCollection form)
		{
			var values = form.Get("values");
			var item = new TModel();
			JsonConvert.PopulateObject(values, item);
			var result = HandleValidation(await MainStore.CreateAsync(item), item);
			if (!result.Success)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));
			}
			return Request.CreateResponse(HttpStatusCode.Created, item);
		}

		protected abstract TKey GetFormDataKey(FormDataCollection form);
		
		public async virtual Task<HttpResponseMessage> UpdateItem(FormDataCollection form)
		{
			var values = form.Get("values");

			var item = MainStore.GetByKey(GetFormDataKey(form));
			var result = HandleValidation(await MainStore.UpdateAsync(item), item);
			if (!result.Success)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));
			}
			return Request.CreateResponse(HttpStatusCode.OK, item);
		}
		
		public async virtual Task<HttpResponseMessage> DeleteItem(TKey key)
		{
			var item = MainStore.GetByKey(key);
			var result = HandleValidation(await MainStore.DeleteAsync(key), item);
			if (!result.Success)
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, GetFullErrorMessage(ModelState));

			return Request.CreateResponse(HttpStatusCode.OK);
		}
	}

	public class BaseApiController<TMainStore, TModel> : BaseApiController<TMainStore, int, TModel>
		where TMainStore : DX.Data.IQueryableDataStore<int, TModel>	
		where TModel : class, new()
	{
		public BaseApiController(): base()
		{

		}

		protected override int GetFormDataKey(FormDataCollection form) => Convert.ToInt32(form.Get("key"));
	}
}