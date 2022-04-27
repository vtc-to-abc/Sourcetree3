using Microsoft.AspNetCore.Components;
using System.Collections.Specialized;
using System.Web;

namespace BlazorClient.Helpers
{
    // add some extension for the NavigationManager, for accessing query string parameters in the url
    // to return the value that a parameter had, to do stuff later?
    public static class ExtensionMethods
    {
        public static NameValueCollection QueryString(this NavigationManager navigationManager)
        {
            return HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);
        }

        public static string QueryString(this NavigationManager navigationManager, string key)
        {
            return navigationManager.QueryString()[key];
        }
    }
}
