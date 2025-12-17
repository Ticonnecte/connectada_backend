using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CidConnectada.Webapi.Code.Extensions
{
    public static class HtmlHelper
    {
        public static IList<string> ExtractImgSrcAttribute(string htmlContent)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);
            IList<string> imgSources = htmlDocument.DocumentNode.QuerySelectorAll("img").Select(x => x.Attributes["src"].Value).ToList();
            return imgSources;
        }

    }
}
