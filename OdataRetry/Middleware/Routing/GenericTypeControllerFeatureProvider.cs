using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using OdataRetry.Common;
using OdataRetry.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OdataRetry.Middleware.Routing
{
    public class GenericTypeControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            foreach (Type item in MfgCommon.GetTypesInNamespace())
            {
                var entityType = item;
                var typeName = entityType.Name + "Controller";
                if (!feature.Controllers.Any(t => t.Name == typeName))
                {
                    var controllerType = typeof(ManufacturingController<>)
                        .MakeGenericType(entityType)
                        .GetTypeInfo();

                    feature.Controllers.Add(controllerType);
                }
            }
        }
    }
}
