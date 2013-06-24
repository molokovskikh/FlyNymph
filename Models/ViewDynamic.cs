using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
//using System.Web.WebPages;
using System.Web.Mvc.Html;

namespace FlyNymph.Models
{
    public static class UtilsView
    {
        /// <summary>
        /// Привязка View как секций
        /// </summary>
        /// <param name="names">имя(на) предствлений в корне Views, для вывода в одноименную секцию мастер слоя</param>
        /// <returns></returns>
        internal static Action<TextWriter> _RenderSections(System.Web.WebPages.WebPageBase page, params object[] names)
        {
            return delegate(TextWriter wr)
            {
                if (names == null) return;


                foreach (string name in names)
                {
                    Type t = page.GetType();
                    for (int i = 0; i < 10; i++) t = t != null && !t.FullName.Equals("System.Web.WebPages.WebPageBase") ? t.BaseType : t;

                    System.Reflection.PropertyInfo pis =
                        t.GetProperty("SectionWritersStack", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    if (pis != null)
                    {
                        Stack<Dictionary<string, System.Web.WebPages.SectionWriter>> swss = pis.GetValue(page) as Stack<Dictionary<string, System.Web.WebPages.SectionWriter>>;
                        if (swss != null)
                        {
                            Dictionary<string, System.Web.WebPages.SectionWriter> sws = swss.Last();
                            if (sws != null)
                                sws.Add(name, delegate()
                                {
                                    System.Web.Mvc.WebViewPage wp = page as System.Web.Mvc.WebViewPage;
                                    if (wp != null)
                                    {
                                        Exception exc = null;
                                        try
                                        {
                                            wp.Html.RenderPartial(string.Format(@"~/Views/{0}.cshtml", name));
                                        }
                                        catch (Exception e)
                                        {
                                            exc = e;
                                            try
                                            {
                                                wp.Html.RenderPartial(string.Format(@"~/Views/{0}.vbhtml", name));
                                                exc = null;
                                            }
                                            catch (InvalidOperationException)
                                            {
                                            }
                                            catch (Exception exc2)
                                            {
                                                exc = exc2;
                                            }

                                            if (exc != null)
                                                try
                                                {
                                                    wp.Html.RenderPartial(string.Format(@"~/Views/{0}.aspx", name));
                                                    exc = null;
                                                }
                                                catch (InvalidOperationException)
                                                {
                                                }
                                                catch (Exception exc3)
                                                {
                                                    exc = exc3;
                                                }
                                            if (exc != null) throw;
                                        }
                                    }
                                }
                                       );
                        }
                    }
                }
            };
        }


        public static System.Web.WebPages.HelperResult RenderSections(this System.Web.Mvc.WebViewPage<dynamic> page, params object[] names)
        {
            return new System.Web.WebPages.HelperResult(_RenderSections(page, names));
        }
       
    }
    
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