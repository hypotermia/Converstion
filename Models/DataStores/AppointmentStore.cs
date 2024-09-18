using DevExpress.Xpo;
using DX.Data.Xpo.Mapster;
using DXMVCTestApplication.Models.XPO;
using MapsterMapper;

namespace DXMVCTestApplication.Models
{
	public class AppointmentStore : XPMapsterStore<int, Appointment, XPAppointment>
	{
		public AppointmentStore(IDataLayer dataLayer) 
			: base(dataLayer, new Mapper(), new XPAppointmentValidator())
		{

		}

		public override string KeyField => nameof(XPAppointment.Oid);
		public override int ModelKey(Appointment model) => model.Oid;
		public override void SetModelKey(Appointment model, int key) => model.Oid = key;
		protected override int DBModelKey(XPAppointment model) => model.Oid;
	}
}