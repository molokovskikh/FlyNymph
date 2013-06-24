using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GalleryServerPro.Data
{
	[Table("gs_GalleryControlSetting")]
	public class GalleryControlSettingDto
	{
		[Key]
		public int GalleryControlSettingId
		{
			get;
			set;
		}

		public string ControlId
		{
			get;
			set;
		}

		public string SettingName
		{
			get;
			set;
		}

		[MaxLength]
		public string SettingValue
		{
			get;
			set;
		}
	}
}
