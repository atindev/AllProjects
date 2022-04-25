using Microsoft.AspNetCore.Mvc.ApplicationModels;
using OdataRetry.Controllers;
using System;

namespace OdataRetry.Middleware.Routing
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class GenericControllerNameAttribute : Attribute, IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.GetGenericTypeDefinition() == typeof(ManufacturingController<>))
            {
                var entityType = controller.ControllerType.GenericTypeArguments[0];
                controller.ControllerName = entityType.Name;
                controller.RouteValues["Controller"] = entityType.Name;
            }
        }
    }
}
