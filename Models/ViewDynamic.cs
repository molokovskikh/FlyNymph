using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
//using System.Web.WebPages;
//using System.Web.Mvc.Html;

namespace FlyNymph.Models
{   
    public class DynamicViewPage<T> : System.Web.Mvc.WebViewPage<T>
    {
       public new dynamic Model {get;private set;}

       protected override void SetViewData(System.Web.Mvc.ViewDataDictionary viewData)
       {
           base.SetViewData(viewData);
           Model = new DynamicObject(ViewData.Model);
       }
       public override void Execute()
       {
       }
        
       public System.Web.WebPages.HelperResult RenderSections(params object[] names)
       {
           return new System.Web.WebPages.HelperResult(UtilsView._RenderSections(this, names));
       }
    }
   
    public class DynamicObject:System.Dynamic.DynamicObject
    {
        public static DynamicObject Wrap(object targetObject)
        {
            return (targetObject == null) ? null : new DynamicObject(targetObject);
        }

        public DynamicObject(object innerObject)
        {
            if (innerObject == null) return;
            Type t =  innerObject.GetType();
            if ( (t.IsArray && t.Equals(typeof(object[]))) || t.GetInterface("System.Collections.IColection")!=null || t.GetInterface("System.Collections.IList") != null)
            {
                //(innerObject as IEnumerable<object>).Aggregate(0, (f, e) => { dict.Add(new DynamicObject(e)); return f; });
                return;
            }

            System.Reflection.PropertyInfo[] pis = t.GetProperties();
            if (pis != null)
            {
                foreach (System.Reflection.PropertyInfo pi in pis)
                {
                    if (pi.PropertyType.Equals(typeof(object)))
                        dict.Add(pi.Name, new DynamicObject(pi.GetValue(innerObject)));
                    else
                    {
                        if (pi.PropertyType.Equals(typeof(object[])))
                        {
                            List<DynamicObject> array = new List<DynamicObject>();
                            foreach (var el in (pi.GetValue(innerObject) as object[]))
                            {
                                array.Add(new DynamicObject(el));
                            }
                            dict.Add(pi.Name, array);
                        }
                        else
                            dict.Add(pi.Name, pi.GetValue(innerObject));
                    }
                }
            }
        }

        Dictionary<string, object> dict = new Dictionary<string, object>();
        public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
        {
            if (dict.ContainsKey(binder.Name))
            {
                result = dict[binder.Name];
                return true;
            }
            result = null;
            return true;
        }

        public override bool TrySetMember(System.Dynamic.SetMemberBinder binder, object value)
        {
            dict[binder.Name] = value;
            return true;
        }
        

    }
}