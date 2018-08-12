// © Alexander Kozlenko. Licensed under the MIT License.

namespace System.Data.JsonRpc
{
    /// <summary>Represents a JSON-RPC message.</summary>
    public abstract class JsonRpcMessage
    {
        private readonly JsonRpcId _id;

        private protected JsonRpcMessage()
        {
        }

        private protected JsonRpcMessage(in JsonRpcId id)
        {
            _id = id;
        }

        /// <summary>Gets the JSON-RPC message identifier.</summary>
        public ref readonly JsonRpcId Id
        {
            get => ref _id;
        }
    }
}