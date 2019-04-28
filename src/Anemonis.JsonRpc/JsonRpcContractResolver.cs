// © Alexander Kozlenko. Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;

namespace Anemonis.JsonRpc
{
    /// <summary>Represents a JSON-RPC message contract resolver.</summary>
    public sealed class JsonRpcContractResolver : IJsonRpcContractResolver
    {
        private readonly SpinLock _operationLock = new SpinLock(false);

        private readonly IDictionary<string, JsonRpcRequestContract> _staticRequestContracts = new Dictionary<string, JsonRpcRequestContract>(StringComparer.Ordinal);
        private readonly IDictionary<string, JsonRpcResponseContract> _staticResponseContracts = new Dictionary<string, JsonRpcResponseContract>(StringComparer.Ordinal);
        private readonly IDictionary<JsonRpcId, JsonRpcResponseContract> _dynamicResponseContracts = new Dictionary<JsonRpcId, JsonRpcResponseContract>();
        private readonly IDictionary<JsonRpcId, string> _staticResponseBindings = new Dictionary<JsonRpcId, string>();

        /// <summary>Initializes a new instance of the <see cref="JsonRpcContractResolver" /> class.</summary>
        public JsonRpcContractResolver()
        {
        }

        /// <summary>Gets the JSON-RPC request contract.</summary>
        /// <param name="method">The name of a JSON-RPC method.</param>
        /// <returns>The corresponding request contract or <see langword="null" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="method" /> is <see langword="null" />.</exception>
        public JsonRpcRequestContract GetRequestContract(string method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var operationLockTaken = false;

            _operationLock.Enter(ref operationLockTaken);
            _staticRequestContracts.TryGetValue(method, out var contract);

            if (operationLockTaken)
            {
                _operationLock.Exit();
            }

            return contract;
        }

        /// <summary>Gets the JSON-RPC response contract.</summary>
        /// <param name="messageId">The JSON-RPC message identifier.</param>
        /// <returns>The corresponding response contract or <see langword="null" />.</returns>
        public JsonRpcResponseContract GetResponseContract(in JsonRpcId messageId)
        {
            var operationLockTaken = false;

            _operationLock.Enter(ref operationLockTaken);

            if (!_dynamicResponseContracts.TryGetValue(messageId, out var contract))
            {
                if (_staticResponseBindings.TryGetValue(messageId, out var method) && (method != null))
                {
                    _staticResponseContracts.TryGetValue(method, out contract);
                }
            }
            if (operationLockTaken)
            {
                _operationLock.Exit();
            }

            return contract;
        }

        /// <summary>Adds the specified JSON-RPC request contract.</summary>
        /// <param name="method">The name of a JSON-RPC method.</param>
        /// <param name="contract">The JSON-RPC request contract.</param>
        /// <exception cref="ArgumentNullException"><paramref name="method" /> or <paramref name="contract" /> is <see langword="null" />.</exception>
        public void AddRequestContract(string method, JsonRpcRequestContract contract)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var operationLockTaken = false;

            _operationLock.Enter(ref operationLockTaken);
            _staticRequestContracts[method] = contract;

            if (operationLockTaken)
            {
                _operationLock.Exit();
            }
        }

        /// <summary>Adds the specified JSON-RPC response contract.</summary>
        /// <param name="method">The name of a JSON-RPC method.</param>
        /// <param name="contract">The JSON-RPC response contract.</param>
        /// <exception cref="ArgumentNullException"><paramref name="method" /> or <paramref name="contract" /> is <see langword="null" />.</exception>
        public void AddResponseContract(string method, JsonRpcResponseContract contract)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var operationLockTaken = false;

            _operationLock.Enter(ref operationLockTaken);
            _staticResponseContracts[method] = contract;

            if (operationLockTaken)
            {
                _operationLock.Exit();
            }
        }

        /// <summary>Adds the specified JSON-RPC response contract.</summary>
        /// <param name="messageId">The JSON-RPC message identifier.</param>
        /// <param name="contract">The JSON-RPC response contract.</param>
        /// <exception cref="ArgumentNullException"><paramref name="contract" /> is <see langword="null" />.</exception>
        public void AddResponseContract(in JsonRpcId messageId, JsonRpcResponseContract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var operationLockTaken = false;

            _operationLock.Enter(ref operationLockTaken);
            _dynamicResponseContracts[messageId] = contract;

            if (operationLockTaken)
            {
                _operationLock.Exit();
            }
        }

        /// <summary>Adds the specified JSON-RPC response binding.</summary>
        /// <param name="messageId">The JSON-RPC message identifier.</param>
        /// <param name="method">The name of a JSON-RPC method.</param>
        /// <exception cref="ArgumentNullException"><paramref name="method" /> is <see langword="null" />.</exception>
        public void AddResponseBinding(in JsonRpcId messageId, string method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var operationLockTaken = false;

            _operationLock.Enter(ref operationLockTaken);
            _staticResponseBindings[messageId] = method;

            if (operationLockTaken)
            {
                _operationLock.Exit();
            }
        }

        /// <summary>Removes the corresponding JSON-RPC request contract.</summary>
        /// <param name="method">The name of a JSON-RPC method.</param>
        /// <exception cref="ArgumentNullException"><paramref name="method" /> is <see langword="null" />.</exception>
        public void RemoveRequestContract(string method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var operationLockTaken = false;

            _operationLock.Enter(ref operationLockTaken);
            _staticRequestContracts.Remove(method);

            if (operationLockTaken)
            {
                _operationLock.Exit();
            }
        }

        /// <summary>Removes the corresponding JSON-RPC response contract.</summary>
        /// <param name="method">The name of a JSON-RPC method.</param>
        /// <exception cref="ArgumentNullException"><paramref name="method" /> is <see langword="null" />.</exception>
        public void RemoveResponseContract(string method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var operationLockTaken = false;

            _operationLock.Enter(ref operationLockTaken);
            _staticResponseContracts.Remove(method);

            if (operationLockTaken)
            {
                _operationLock.Exit();
            }
        }

        /// <summary>Removes the corresponding JSON-RPC response contract.</summary>
        /// <param name="messageId">The JSON-RPC message identifier.</param>
        public void RemoveResponseContract(in JsonRpcId messageId)
        {
            var operationLockTaken = false;

            _operationLock.Enter(ref operationLockTaken);
            _dynamicResponseContracts.Remove(messageId);

            if (operationLockTaken)
            {
                _operationLock.Exit();
            }
        }

        /// <summary>Removes the corresponding JSON-RPC response binding.</summary>
        /// <param name="messageId">The JSON-RPC message identifier.</param>
        public void RemoveResponseBinding(in JsonRpcId messageId)
        {
            var operationLockTaken = false;

            _operationLock.Enter(ref operationLockTaken);
            _staticResponseBindings.Remove(messageId);

            if (operationLockTaken)
            {
                _operationLock.Exit();
            }
        }

        /// <summary>Removes all JSON-RPC request contracts.</summary>
        public void ClearRequestContracts()
        {
            var operationLockTaken = false;

            _operationLock.Enter(ref operationLockTaken);
            _staticRequestContracts.Clear();

            if (operationLockTaken)
            {
                _operationLock.Exit();
            }
        }

        /// <summary>Removes all JSON-RPC response contracts.</summary>
        public void ClearResponseContracts()
        {
            var operationLockTaken = false;

            _operationLock.Enter(ref operationLockTaken);
            _staticResponseContracts.Clear();
            _dynamicResponseContracts.Clear();

            if (operationLockTaken)
            {
                _operationLock.Exit();
            }
        }

        /// <summary>Removes all JSON-RPC response bindings.</summary>
        public void ClearResponseBindings()
        {
            var operationLockTaken = false;

            _operationLock.Enter(ref operationLockTaken);
            _staticResponseBindings.Clear();

            if (operationLockTaken)
            {
                _operationLock.Exit();
            }
        }
    }
}
