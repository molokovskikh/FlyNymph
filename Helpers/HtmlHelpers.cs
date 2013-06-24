﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;

namespace FlyNymph.Helpers
{
    public static class UtilsSection
    {      
        public static MvcHtmlString LoginzaInit(this HtmlHelper html)
        {
            return new MvcHtmlString(@"<script src=""//loginza.ru/js/widget.js"" type=""text/javascript""></script>");
        }

        public static MvcHtmlString Loginza(this HtmlHelper html)
        {
            return html.Loginza((string)null,(string) null, (string[])null);
        }

        public static MvcHtmlString Loginza(this HtmlHelper html, string action, string controller)
        {
            return html.Loginza(action,controller,(string) null,(string[])null);
        }

        public static MvcHtmlString Loginza(this HtmlHelper html,params string[] providers)
        {
            return html.Loginza(null, null, providers);
        }
        public static MvcHtmlString Loginza(this HtmlHelper html, string TextLink)
        {
            return html.Loginza((string)null, TextLink,(string[]) null);
        }
        

        public static MvcHtmlString Loginza(this HtmlHelper html, string TextLink, params string[] providers)
        {            
            return html.Loginza(null, TextLink, providers);
        }

        public static MvcHtmlString Loginza(this HtmlHelper html,string action,string controller,string TextLink)
        {
            return html.Loginza(action,controller,TextLink,(string[]) null);
        }

        public static MvcHtmlString Loginza(this HtmlHelper html, string action, string controller, string TextLink, params string[] providers)
        {
            return html.Loginza(string.Format("/{1}/{0}", action, controller), TextLink, providers);            
        }

        public static MvcHtmlString Loginza(this HtmlHelper html, string uri, string TextLink, params string[] providers)
        {
            //Если это не uri, значит это action
            if (!string.IsNullOrEmpty(uri) && uri.Split(new string[] { "/" },StringSplitOptions.RemoveEmptyEntries).Length < 2)
            {                
                return html.Loginza(string.Format("/{1}/{0}", uri, TextLink), string.Empty, providers);
            }
            uri = uri ?? "/services/loginza";
            
            TextLink = string.IsNullOrEmpty(TextLink)? @"<img src=""http://loginza.ru/img/sign_in_button_gray.gif"" alt=""Войти через loginza""/>":TextLink;
            Uri url = html.ViewContext.HttpContext.Request.Url;
            uri = uri.Trim();
            uri = uri.StartsWith("/") ? uri : "/"+uri;
            
            string pProvider=(providers != null && providers.Length==1&&providers[0].Split(new string[]{","},StringSplitOptions.RemoveEmptyEntries).Length==1)?string.Format(@"&provider={0}",providers[0]):string.Empty;
            string pProvidersSet = (providers != null && providers.Length > 1 && string.IsNullOrEmpty(pProvider)) ?
                @"&providers_set="+ providers.Aggregate(string.Empty,(a,b)=>{  a+=b+((providers.Last().Equals(b)&&a.Split(new string[] {","},StringSplitOptions.RemoveEmptyEntries).Length==providers.Length-1)?string.Empty:","); return a;  })
                :string.Empty;

            return new MvcHtmlString(
                string.Format(@"<a href=""https://www.loginza.ru/api/widget?token_url={0}{1}{2}"" class=""loginza"">{3}</a>",
                    HttpUtility.UrlEncode(string.Format("{0}://{1}{2}",url.Scheme,url.Authority,uri)),
                    pProvidersSet,
                    pProvider,
                    TextLink)
            );
        }
    }
}