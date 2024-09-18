using DevExpress.Xpo;
using DX.Data.Xpo.Mapster;
using DXMVCTestApplication.Models.XPO;
using MapsterMapper;


namespace DXMVCTestApplication.Models
{
	public class PatientStore : XPMapsterStore<int, Patient,XPPatient>
	{

		public PatientStore(IDataLayer dataLayer) 
			: base(dataLayer, new Mapper(), new XPPatientValidator())
		{

		}

		public override string KeyField => nameof(XPPatient.Oid);
		public override int ModelKey(Patient model) => model.Oid;
		public override void SetModelKey(Patient model, int key) => model.Oid = key;
		protected override int DBModelKey(XPPatient model) => model.Oid;
	}


}