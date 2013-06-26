using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GalleryServerPro.Data;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.Provider;
using GalleryServerPro.Web;

namespace FlyNymph.Models
{
    abstract class A
    {

    }
    
    class B : A
    {
    }
  

    public static class CarouselData
    {
        


        public static object GetByGallery(string name)
        {
            
            //System.GC.SuppressFinalize();
            IGallery g = DataProviderManager.Provider.Gallery_GetGalleries(new GalleryCollection()).Where(x => x.Description == name).FirstOrDefault();
            Func<int, int, DisplayObjectType, string> fMoUrl = delegate(int galleryId, int mediaObjectId,DisplayObjectType displayType)
            {
                return string.Concat(Utils.GalleryRoot,
                               "/handler/getmedia.ashx?",
                               string.Format("moid={0}&dt={1}&g={2}", mediaObjectId, (int)displayType, galleryId));
            };

            return new
            {
                CarouselData =

                    (g != null) ?
                    g.Albums.Where(w => DataProviderManager.Provider.Album_GetAlbumById(w.Key).AlbumParentId> 0 &&
                                        DataProviderManager.Provider.Album_GetChildAlbumIdsById(w.Key).Count()==0
                                   )
                            .Aggregate(new List<DynamicObject>(), (f, e) =>
                    {
                        DataProviderManager.Provider.Album_GetChildMediaObjectsById(e.Key, false).Aggregate(f, (ff, mo) =>
                        {

                            ff.Add(new DynamicObject(
                                new
                                {
                                    Active = e.Equals(g.Albums.First()),
                                    url = fMoUrl(g.GalleryId, mo.MediaObjectId, DisplayObjectType.Optimized),
                                    description = mo.Title
                                }));
                            return ff;
                        });
                        return f;
                    })
                    : null
            };
        }    



       public static object GetStepByStep(int ? id)
        {
            IGallery g = DataProviderManager.Provider.Gallery_GetGalleries(new GalleryCollection()).Where(x => x.Description =="stepbystep").FirstOrDefault();
            if (g == null) return null; 
           Func<int, int, DisplayObjectType, string> fMoUrl = delegate(int galleryId, int mediaObjectId,DisplayObjectType displayType)
            {
                return string.Concat(Utils.GalleryRoot,
                               "/handler/getmedia.ashx?",
                               string.Format("moid={0}&dt={1}&g={2}", mediaObjectId, (int)displayType, galleryId));
            };
            
            
            var albums = g.Albums.Where(w => DataProviderManager.Provider.Album_GetAlbumById(w.Key).AlbumParentId > 0);
            int _min = albums.Min(x => x.Key);
            int _max = albums.Max(x => x.Key);
                
            int pos_carousel = -1;
            int.TryParse(HttpContext.Current.Session["album_current"] as string,out pos_carousel);            
            if(pos_carousel <= 0 && ( (id.HasValue && id.Value > 0) || int.TryParse(HttpContext.Current.Request.Params["id"] as string, out pos_carousel)))
            {
                pos_carousel = (pos_carousel<=0 && id.HasValue && id.Value > 0)? id.Value : pos_carousel;
                HttpContext.Current.Session["album_current"] = pos_carousel;
            }

            if (pos_carousel <= 0)
            {
                pos_carousel = (pos_carousel < _min) ? _min: pos_carousel;
                while ((pos_carousel = _min+ new Random().Next(_max-_min)) > 0)
                    if (pos_carousel >= _min)
                        break;
                HttpContext.Current.Session["album_current"] = pos_carousel;
            }
            return new
            {
                CarouselData =
                    (g != null) ?
                    albums.Aggregate(new List<DynamicObject>(), (f, e) =>
                    {
                           AlbumDto album = DataProviderManager.Provider.Album_GetAlbumById(e.Key);
                           MediaObjectDto mo = DataProviderManager.Provider.MediaObject_GetMediaObjectById(album.ThumbnailMediaObjectId,false);
                           f.Add(new DynamicObject(
                                new
                                {
                                    id = e.Key,
                                    Active = e.Key.Equals(pos_carousel),
                                    url = fMoUrl(g.GalleryId, mo.MediaObjectId, DisplayObjectType.Optimized),
                                    description = album.Title
                                }));
                        return f;
                    })
                    : null
                ,
                StepByStepDetails =

                    (g != null) ?
                    g.Albums.Where(w=>w.Key==pos_carousel).Aggregate(new List<DynamicObject>(), (f, e) =>
                    {
                        DataProviderManager.Provider.Album_GetChildMediaObjectsById(e.Key, false).Aggregate(f, (ff, mo) =>
                        {
                            ff.Add(new DynamicObject(
                                new
                                {
                                    album = e.Key,
                                    url = fMoUrl(g.GalleryId, mo.MediaObjectId, DisplayObjectType.Optimized),
                                    description = mo.Title
                                }));
                            return ff;
                        });
                        return f;
                    })
                    : null
            };
        }
    }
}