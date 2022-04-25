using Microsoft.AspNetCore.Routing;

namespace OdataLangTry
{
    public class NavigationIndexRoutingConvention //: IODataRoutingConvention
    {
        //public IEnumerable SelectAction(RouteContext routeContext)
        public void SelectAction(RouteContext routeContext)
        {
            //// Get a IActionDescriptorCollectionProvider from the global service provider
            //IActionDescriptorCollectionProvider actionCollectionProvider = routeContext.HttpContext.RequestServices.GetRequiredService();
            //Contract.Assert(actionCollectionProvider != null);

            //// Get OData path from HttpContext
            //Microsoft.AspNet.OData.Routing.ODataPath odataPath = routeContext.HttpContext.ODataFeature().Path;
            //HttpRequest request = routeContext.HttpContext.Request;

            //// Handle this type of GET requests: /odata/Orders(1)/OrderRows(1)
            //if (request.Method == "GET" && odataPath.PathTemplate.Equals("~/entityset/key/navigation/key"))
            //{
            //    // Find correct controller
            //    string controllerName = odataPath.Segments[3].Identifier;
            //    IEnumerable actionDescriptors = actionCollectionProvider
            //        .ActionDescriptors.Items.OfType()
            //        .Where(c => c.ControllerName == controllerName);

            //    if (actionDescriptors != null)
            //    {
            //        // Find correct action
            //        string actionName = "Get";
            //        var matchingActions = actionDescriptors
            //            .Where(c => String.Equals(c.ActionName, actionName, StringComparison.OrdinalIgnoreCase) && c.Parameters.Count == 2)
            //            .ToList();
            //        if (matchingActions.Count > 0)
            //        {
            //            // Set route data values
            //            var keyValueSegment = odataPath.Segments[3] as KeySegment;
            //            var keyValueSegmentKeys = keyValueSegment?.Keys?.FirstOrDefault();
            //            routeContext.RouteData.Values[ODataRouteConstants.Key] = keyValueSegmentKeys?.Value;

            //            var relatedKeyValueSegment = odataPath.Segments[1] as KeySegment;
            //            var relatedKeyValueSegmentKeys = relatedKeyValueSegment?.Keys?.FirstOrDefault();
            //            routeContext.RouteData.Values[ODataRouteConstants.RelatedKey] = relatedKeyValueSegmentKeys?.Value;

            //            // Return correct action
            //            return matchingActions;
            //        }
            //    }
            //}

            //// Not a match
            //return null;
        }
    }

}
