using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GalleryServerPro.Data
{
	[Table("gs_BrowserTemplate")]
	public class BrowserTemplateDto
	{
		[Key]
		public int BrowserTemplateId
		{
			get;
			set;
		}

		public string MimeType
		{
			get;
			set;
		}

		public string BrowserId
		{
			get;
			set;
		}

		[MaxLength]
		public string HtmlTemplate
		{
			get;
			set;
		}

		[MaxLength	]
		public string ScriptTemplate
		{
			get;
			set;
		}
	}
}
