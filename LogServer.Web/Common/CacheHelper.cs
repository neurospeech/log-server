using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace System.Web.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    public static class CachedUrlHelper
    {

        private static string _v = null;
        private static string version =>
            _v ?? (_v = typeof(CachedUrlHelper).Assembly.GetName().Version.ToString());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IHtmlString Cached(string path) {
            return new HtmlString($"/cached/{version}{path}");
        }

    }
}