using System;

namespace SignalClientConsole
{
    public class NotificationModel
    {
        /// <summary>
        /// Gets or sets the name of the pu.
        /// </summary>
        /// <value>
        /// The name of the pu.
        /// </value>
        public string PuName { get; set; }

        /// <summary>
        /// Gets or sets the name of the room.
        /// </summary>
        /// <value>
        /// The name of the room.
        /// </value>
        public string RoomName { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; } = "Pinged by";

        /// <summary>
        /// Gets or sets the name of the notification.
        /// </summary>
        /// <value>
        /// The name of the notification.
        /// </value>
        public string NotificationName { get; set; }

        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        /// <value>
        /// The date time.
        /// </value>
        public string ShowDateTime
        {
            get
            {
                return NotificationDateTime.ToString("dd-MMM-yyyy hh:mm");
            }
        }

        /// <summary>
        /// Gets or sets the notification date time.
        /// </summary>
        /// <value>
        /// The notification date time.
        /// </value>
        public DateTime NotificationDateTime { get; set; } = DateTime.Now;
    }
}
