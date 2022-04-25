using System;
using WAS.Application.Interface.Exceptions;

namespace WAS.Application.Common.Exceptions
{
    /// <summary>
    /// Custom InternalServerErrorException.
    /// </summary>
    [Serializable]
    public class InternalServerErrorException : Exception, IRequestException
    {
        /// <summary>
        /// Initialize a new InternalServerErrorException.
        /// </summary>
        public InternalServerErrorException() : base()
        {
        }

        /// <summary>
        /// Initialize a new InternalServerErrorException with an exception message.
        /// </summary>
        public InternalServerErrorException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initialize a new InternalServerErrorException with an exception message and inner exception.
        /// </summary>
        public InternalServerErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected InternalServerErrorException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
