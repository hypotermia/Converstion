using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace DXMVCTestApplication.Models.XPO
{

	public partial class XPPatient
	{
		public XPPatient(Session session) : base(session) { }
		public override void AfterConstruction() { base.AfterConstruction(); }
		
		[PersistentAlias("[Appointments][[Status] = 1 AND [Date] < now()].Max([Date])")]
		public DateTime LastVisit
		{
			get { 
				object o = EvaluateAlias(nameof(LastVisit));
				if (o == null || o == DBNull.Value) return DateTime.MinValue;
				return DateTime.Now; 
			}
		}
		
		[PersistentAlias("[Appointments][[Date] > now()].Min([Date])")]
		public DateTime NextVisit {
			get
			{
				object o = EvaluateAlias(nameof(NextVisit));
				if (o == null || o == DBNull.Value) return DateTime.MinValue;
				return (DateTime)o;
			}
		}
		
	}

}
