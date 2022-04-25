using Newtonsoft.Json;

namespace PackTesting.Model
{
    class ApiResult<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ApiResult{T}"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("Success")]
        public bool Success { get; set; } = false;

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [JsonProperty("Message")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        [JsonProperty("Result")]
        public T Result { get; set; }
    }
}
