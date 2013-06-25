using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
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
  
}