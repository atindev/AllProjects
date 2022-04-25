using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Infrastructure.OData
{
    public class WasEntityDataModel
    {
        public IEdmModel GetEntityDataModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.Namespace = "WAS";
            builder.ContainerName = "WASContainer";

            builder.EntitySet<Domain.Entities.Subscription>("Subscription");
            builder.EntitySet<Domain.Entities.Location>("Location");
            builder.EntitySet<Domain.Entities.Group>("Group");
            builder.EntitySet<Domain.Entities.Notification>("Notification");
            builder.EntitySet<Domain.Entities.Event>("Event");
            builder.EntitySet<Domain.Entities.DeliveryReportText>("DeliveryReportText");
            builder.EntitySet<Domain.Entities.DeliveryReportVoice>("DeliveryReportVoice");
            builder.EntitySet<Domain.Entities.DeliveryReportWhatsApp>("DeliveryReportWhatsApp");

            return builder.GetEdmModel();
        }
    }
}
