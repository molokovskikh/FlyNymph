using System;
using System.Collections.Generic;
using System.Text;
using GalleryServerPro.Business.Interfaces;

namespace GalleryServerPro.Business.NullObjects
{
	/// <summary>
	/// Represents a <see cref="IDisplayObject" /> that is equivalent to null. This class is used instead of null to prevent 
	/// <see cref="NullReferenceException" /> errors if the calling code accesses a property or executes a method.
	/// </summary>
	public class NullDisplayObject : IDisplayObject
	{
   
		public IGalleryObject Parent
		{
			get
			{
				return new NullGalleryObject();
			}
			set
			{
			}
		}
		
		public int Width
		{
			get
			{
				return int.MinValue;
			}
			set
			{
			}
		}

		public int Height
		{
			get
			{
				return int.MinValue;
			}
			set
			{
			}
		}

		public string FileName
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		public string FileNamePhysicalPath
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		public string TempFilePath
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		public IMimeType MimeType
		{
			get
			{
				return new NullMimeType();
			}
		}

		public DisplayObjectType DisplayType
		{
			get
			{
				return DisplayObjectType.Unknown;
			}
			set
			{
			}
		}

		public int MediaObjectId
		{
			get
			{
				return int.MinValue;
			}
			set
			{
			}
		}

		public IDisplayObjectCreator DisplayObjectCreator
		{
			get
			{
				return new NullDisplayObjectCreator();
			}
			set
			{
			}
		}

		public string ExternalHtmlSource
		{
			get
			{
				return string.Empty;
			}
			set
			{
			}
		}

		public MimeTypeCategory ExternalType
		{
			get
			{
				return MimeTypeCategory.NotSet;
			}
			set
			{
			}
		}

		public void GenerateAndSaveFile()
		{
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		public System.IO.FileInfo FileInfo
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public System.Drawing.Bitmap Bitmap
		{
			get
			{
				return null;
			}
		}
		
		public int FileSizeKB
		{
			get
			{
				return int.MinValue;
			}
			set
			{
			}
		}
	}
}
