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
    public ActionResult Index()
    {
        //data.CarouselData = new object[] { new { url = "http://www.codeproject.com/App_Themes/CodeProject/Img/logo250x135.gif" } };
         GalleryServerPro.Business.Interfaces.IGallery g= GalleryServerPro.Provider.DataProviderManager.Provider.Gallery_GetGalleries(new GalleryServerPro.Business.GalleryCollection()).Where(x=>x.Description=="fly-nymph").FirstOrDefault();
         
         List<object> items = new List<object>();
         foreach (var a in g.Description)
         {             
             items.Add(new { description = g.Description });
         }
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


