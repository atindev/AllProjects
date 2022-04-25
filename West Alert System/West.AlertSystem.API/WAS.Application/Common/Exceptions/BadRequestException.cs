using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Interface.Exceptions;

namespace WAS.Application.Common.Exceptions
{
    /// <summary>
    /// Custom BadRequestException.
    /// </summary>
    [Serializable]
    public class BadRequestException : Exception, IRequestException
    {
        /// <summary>
        /// Initialize a new BadRequestException.
        /// </summary>
        public BadRequestException() : base()
        {
        }

        /// <summary>
        /// Initialize a new BadRequestException with an exception message.
        /// </summary>
        public BadRequestException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initialize a new BadRequestException with an exception message and inner exception.
        /// </summary>
        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected BadRequestException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
