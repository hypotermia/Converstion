using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace DXMVCTestApplication.Models.XPO
{

	public partial class XPAppointment
	{
		public XPAppointment(Session session) : base(session) { }
		public override void AfterConstruction() { base.AfterConstruction(); }
		private AppointmentStatus _status = AppointmentStatus.Open;
		[Indexed(Name = @"iAppointment_Status")]
		[Persistent(@"Status")]
		public AppointmentStatus Status
		{
			get { return _status; }
			set { SetPropertyValue(nameof(Status), ref _status, value); }
		}


		[PersistentAlias("[Patient.Oid]")]
		public int PatientId
		{
			get => Convert.ToInt32(EvaluateAlias(nameof(PatientId)));
		}

		[PersistentAlias("[Patient.FullName]")]
		public string PatientName
		{
			get => (string)EvaluateAlias(nameof(PatientName));
		}

	}

}
