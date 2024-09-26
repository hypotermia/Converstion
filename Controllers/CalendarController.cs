using DevExpress.Web.Mvc;
using DevExpress.Xpo;
using DevExtreme.AspNet.Mvc;
using DXMVCTestApplication.Models;
using DXMVCTestApplication.Models.XPO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            // Mengambil janji temu yang diinsert, update, dan delete
            var insertedAppointments = SchedulerExtension.GetAppointmentsToInsert<Appointment>("appointments",
                appts,
                null,
                GetAppointmentsStorage(),
                null);
            await store.CreateAsync(insertedAppointments);

            var updatedAppointments = SchedulerExtension.GetAppointmentsToUpdate<Appointment>("appointments",
                appts,
                null,
                GetAppointmentsStorage(),
                null);
            await store.UpdateAsync(updatedAppointments);

            var removedAppointments = SchedulerExtension.GetAppointmentsToRemove<Appointment>("appointments",
                appts,
                null,
                GetAppointmentsStorage(),
                null);
            await store.DeleteAsync(removedAppointments);

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
