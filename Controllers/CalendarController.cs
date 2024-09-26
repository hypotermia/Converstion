using DevExpress.Web.Mvc;
using DevExpress.Xpo;
using DXMVCTestApplication.Models;
using DXMVCTestApplication.Models.XPO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DXMVCTestApplication.Controllers
{
    public class CalendarController : BaseController<AppointmentStore, int, Appointment>
    {
		public override string PartialViewName => "SchedulerPartialView";

		public async Task<ActionResult> EditAppointment()
		{
			var data = await UpdateAppointments();			
			return PartialView(PartialViewName, data);
		}
		static async Task<IEnumerable<Appointment>> UpdateAppointments()
		{			
			var store = new AppointmentStore(XpoHelper.GetDataLayer());
			var appts = await store.Query().ToListAsync();
			DX.Data.IDataResult<int, Appointment> result = default;

			Appointment[] insertedAppointments = SchedulerExtension.GetAppointmentsToInsert<Appointment>("appointments",
				appts,
				null,
				GetAppointmentsStorage(),
				null);
			result = await store.CreateAsync(insertedAppointments);
			if (!result.Success)
				throw result.Exception;
			
			Appointment[] updatedAppointments = SchedulerExtension.GetAppointmentsToUpdate<Appointment>("appointments",
				appts,
				null, 
				GetAppointmentsStorage(), 
				null);
			result = await store.UpdateAsync(updatedAppointments);
			if (!result.Success)
				throw result.Exception;

			Appointment[] removedAppointments = SchedulerExtension.GetAppointmentsToRemove<Appointment>("appointments",
				appts,
				null, 
				GetAppointmentsStorage(), 
				null);
			result = await store.DeleteAsync(removedAppointments);
			if (!result.Success)
				throw result.Exception;

			return await store.Query().ToListAsync();
		}

		public static MVCxAppointmentStorage GetAppointmentsStorage()
		{
			MVCxAppointmentStorage storage = new MVCxAppointmentStorage();
			
			storage.Mappings.AppointmentId = "Oid";
			storage.Mappings.Start = "Date";
			storage.Mappings.End = "EndDate";
			storage.Mappings.Subject = "Subject";
			storage.CustomFieldMappings.Add(nameof(Appointment.PatientId), nameof(Appointment.PatientId));
			storage.CustomFieldMappings.Add(nameof(Appointment.PatientName), nameof(Appointment.PatientName));
			return storage;
		}

	}
}