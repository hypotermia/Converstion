using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace DXMVCTestApplication.Models.XPO
{

	public partial class XPPicture
	{
		public XPPicture(Session session) : base(session) { }
		public override void AfterConstruction() { base.AfterConstruction(); }
	}

}
