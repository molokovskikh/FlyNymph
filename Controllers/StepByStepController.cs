using FlyNymph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlyNymph.Controllers
{
    public class StepByStepController : Controller
    {
        //
        // GET: /StetByStep/

        public ActionResult Index(int ? id)
        {            
            return View(CarouselData.GetStepByStep(id));
        }

    }
}
