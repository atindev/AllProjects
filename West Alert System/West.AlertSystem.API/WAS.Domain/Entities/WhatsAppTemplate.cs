using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class WhatsAppTemplate : Entity
    {
        /// <summary>
        /// whatsapp template id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Whatsapp template name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whatsapp template description
        /// </summary>
        public string Description { get; set; }
    }
}
