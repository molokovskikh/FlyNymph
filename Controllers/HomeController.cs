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
        return View(CarouselData.GetByGallery("fly-nymph"));
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


