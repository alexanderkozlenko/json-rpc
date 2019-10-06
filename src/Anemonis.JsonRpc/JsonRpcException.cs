// © Alexander Kozlenko. Licensed under the MIT License.

using System;
using System.Runtime.Serialization;

namespace Anemonis.JsonRpc
{
    /// <summary>Represents an error that occurs during JSON-RPC message processing.</summary>
    public abstract class JsonRpcException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="JsonRpcException" /> class with serialized data.</summary>
        /// <param name="info">The <see cref="SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException"><paramref name="info" /> is <see langword="null" />.</exception>
        /// <exception cref="SerializationException">The class name is <see langword="null" /> or <see cref="Exception.HResult" /> is zero (0).</exception>
        protected JsonRpcException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcException" /> class.</summary>
        protected JsonRpcException()
            : base()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcException" /> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        protected JsonRpcException(string message)
            : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="JsonRpcException" /> class.</summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        protected JsonRpcException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
