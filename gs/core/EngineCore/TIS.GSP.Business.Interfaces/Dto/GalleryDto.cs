using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GalleryServerPro.Data
{
	[Table("gs_Gallery")]
	public class GalleryDto
	{
		[Key]
		public int GalleryId
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public System.DateTime DateAdded
		{
			get;
			set;
		}
	}
}
