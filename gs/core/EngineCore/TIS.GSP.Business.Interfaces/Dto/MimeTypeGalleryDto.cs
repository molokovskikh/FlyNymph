using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GalleryServerPro.Data
{
  [Table("gs_MimeTypeGallery")]
  public class MimeTypeGalleryDto
  {
    [Key]
    public int MimeTypeGalleryId
    {
      get;
      set;
    }

    public int FKGalleryId
    {
      get;
      set;
    }

    public int FKMimeTypeId
    {
      get;
      set;
    }

    public bool IsEnabled
    {
      get;
      set;
    }

    [ForeignKey("FKMimeTypeId")]
    public MimeTypeDto MimeType
    {
      get; 
      set;
    }
  }
}
