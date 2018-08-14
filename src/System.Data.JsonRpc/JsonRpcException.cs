// © Alexander Kozlenko. Licensed under the MIT License.

namespace System.Data.JsonRpc
{
    /// <summary>Represents an error that occurs during JSON-RPC message processing.</summary>
    public abstract class JsonRpcException : Exception
    {
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