using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Text;

namespace MealsToGo.Helpers
{
    public static class BeginFormHelper
    {

        public static IDisposable MyBeginForm(this HtmlHelper html, string action, string controller, FormMethod method)
        {
            var routeValues = new RouteValueDictionary();
            var query = html.ViewContext.HttpContext.Request.QueryString;
            foreach (string key in query)
            {
                routeValues[key] = query[key];
            }
            return html.BeginForm(action, controller, routeValues, FormMethod.Get);
        }

        public static MvcHtmlString QueryAsHiddenFields(this HtmlHelper htmlHelper)
        {
            var result = new StringBuilder();
            var query = htmlHelper.ViewContext.HttpContext.Request.QueryString;
            foreach (string key in query.Keys)
            {
                if ((key != "DistanceDD.SelectedDistanceLimit") && (key != "ChangeLocation") && (key != "Search.PickUpDateSearch") && (key != "Search.FreeSearch"))
                    result.Append(htmlHelper.Hidden(key, query[key]).ToHtmlString());
            }
            return MvcHtmlString.Create(result.ToString());
        }




    }

    }
