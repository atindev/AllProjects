using System;
using WAS.Application.Interface.Exceptions;

namespace WAS.Application.Common.Exceptions
{
    /// <summary>
    /// Custom NotFoundException.
    /// </summary>
    [Serializable]
    public class NotFoundException : Exception, IRequestException
    {
        /// <summary>
        /// Initialize a new NotFoundException.
        /// </summary>
        public NotFoundException() : base()
        {
        }

        /// <summary>
        /// Initialize a new NotFoundException with an exception message.
        /// </summary>
        public NotFoundException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initialize a new NotFoundException with an exception message and inner exception.
        /// </summary>
        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected NotFoundException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
