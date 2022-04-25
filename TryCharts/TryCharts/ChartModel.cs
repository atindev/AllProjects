
using System;

namespace TryCharts
{
    public class ChartModel
    {
        /// <summary>
        /// Gets or sets the status start.
        /// </summary>
        /// <value>
        /// The status start.
        /// </value>
        public DateTimeOffset StatusStart { get; set; }

        /// <summary>
        /// Gets or sets the status end.
        /// </summary>
        /// <value>
        /// The status end.
        /// </value>
        public DateTimeOffset StatusEnd { get; set; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the status description.
        /// </summary>
        /// <value>
        /// The status description.
        /// </value>
        public string StatusDescription { get; set; }

        /// <summary>
        /// Gets or sets the status time span.
        /// </summary>
        /// <value>
        /// The status time span.
        /// </value>
        public double StatusTimeSpan1 {
            get
            {
                return (StatusEnd - StatusStart).TotalMinutes;
            }
                }

        public double StatusTimeSpan { get; set; }

        public string PUName { get; set; }
    }
}
