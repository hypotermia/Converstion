using DevExpress.Xpo.DB;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using DXMVCTestApplication.Models.XPO;
using System.Linq;
using DXMVCTestApplication.Models.DentalModelCode;
using Mapster;
using DXMVCTestApplication.Models;

namespace DXMVCTestApplication
{
	public static class XpoHelper
	{	
		private readonly static object lockObject = new object();

		static volatile IDataLayer dataLayer;
		static IDataLayer DataLayer
		{
			get
			{
				if (dataLayer == null)
				{
					lock (lockObject)
					{
						if (dataLayer == null)
						{
							dataLayer = GetDataLayer();
						}
					}
				}
				return dataLayer;
			}
		}

		public static IDataLayer GetDataLayer()
		{
			XpoDefault.Session = null;
			if (!(XpoDefault.DataLayer is ThreadSafeDataLayer))
			{
				lock (lockObject)
				{
					ConnectionHelper.Connect(AutoCreateOption.DatabaseAndSchema, true);
					var dataLayer = ConnectionHelper.GetDataLayer(AutoCreateOption.DatabaseAndSchema);

					var patients = XPDataFeeder.FeedPatients(dataLayer);
					if (patients != null)
					{
						XPDataFeeder.FeedAppointments(dataLayer, patients);
					}
				}

			}
			var result = ConnectionHelper.GetDataLayer(AutoCreateOption.DatabaseAndSchema);
			
			return result;
		}
	}

	public class ImageValueConverter : ValueConverter
	{
		public override Type StorageType { get { return typeof(byte[]); } }
		public override object ConvertToStorageType(object value)
		{
			if (value == null)
			{
				return null;
			}
			else
			{
				var cnv = new System.Drawing.ImageConverter();
				return cnv.ConvertTo(value, StorageType);
			}
		}
		public override object ConvertFromStorageType(object value)
		{
			if (value == null)
			{
				return null;
			}
			else
			{
				var cnv = new System.Drawing.ImageConverter();
				return cnv.ConvertFrom(value);
			}
		}
	}
}