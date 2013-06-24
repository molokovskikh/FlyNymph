using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using FlyNymph.Models;


/// <summary>
/// Summary description for HomeController
/// </summary>
public class HomeController:Controller
{
    private static string GetMediaObjectUrl(int galleryId, int mediaObjectId, GalleryServerPro.Business.DisplayObjectType displayType)
    {
        //string queryString = String.Format(CultureInfo.InvariantCulture, "moid={1}&aid={2}&mo={3}&mtc={4}&dt={5}&isp={6}", galleryId, mediaObjectId, albumId, Uri.EscapeDataString(mediaObjectPhysicalPath), (int)mimeType.TypeCategory, (int)displayType, isPrivate.ToString());
        string queryString = String.Format(System.Globalization.CultureInfo.InvariantCulture, "moid={0}&dt={1}&g={2}", mediaObjectId, (int)displayType, galleryId);

        // If necessary, encrypt, then URL encode the query string.
        if (GalleryServerPro.Business.AppSetting.Instance.EncryptMediaObjectUrlOnClient)
            queryString = GalleryServerPro.Web.Utils.UrlEncode(GalleryServerPro.Business.HelperFunctions.Encrypt(queryString));

        return string.Concat(GalleryServerPro.Web.Utils.GalleryRoot, "/handler/getmedia.ashx?", queryString);
    }

    public ActionResult Index()
    {
        //data.CarouselData = new object[] { new { url = "http://www.codeproject.com/App_Themes/CodeProject/Img/logo250x135.gif" } };
         GalleryServerPro.Business.Interfaces.IGallery g= GalleryServerPro.Provider.DataProviderManager.Provider.Gallery_GetGalleries(new GalleryServerPro.Business.GalleryCollection()).Where(x=>x.Description=="fly-nymph").FirstOrDefault();
         
         List<object> items = 
         g.Albums.Aggregate(new List<object>(),(f,e)=>
             {
                 GalleryServerPro.Provider.DataProviderManager.Provider.Album_GetChildMediaObjectsById(e.Key,false).Aggregate(f, (ff, mo) =>
                 {
                                      
                     ff.Add(new { url= mo.ExternalHtmlSource,description=mo.Title });
                     return ff;
                 });                 
                 return f;
             });
        return View(items);
        return View(

            new
            {
                Name = "Вася",
                CarouselData =
                new object[] { new { url = "http://www.codeproject.com/App_Themes/CodeProject/Img/logo250x135.gif",description = "Описалово",Active=true },
                               new { url = "http://www.weblancer.net/img/logo.png",description = "Описалово2" }}
            });

    }

    [ChildActionOnly]
    public ActionResult Carousel()
    {

        return PartialView("~/Views/Carousel.cshtml");
    }

    [ChildActionOnly]
    public ActionResult Widgets()
    {
        return PartialView("~/Views/Widgets.cshtml");
    }

}


