// © Alexander Kozlenko. Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;

namespace Anemonis.JsonRpc
{
    /// <summary>Represents a JSON-RPC message contract resolver.</summary>
    public sealed class JsonRpcContractResolver : IJsonRpcContractResolver
    {
        private readonly SpinLock _spinLock = new SpinLock(false);

        private readonly IDictionary<string, JsonRpcRequestContract> _staticRequestContracts = new Dictionary<string, JsonRpcRequestContract>(StringComparer.Ordinal);
        private readonly IDictionary<string, JsonRpcResponseContract> _staticResponseContracts = new Dictionary<string, JsonRpcResponseContract>(StringComparer.Ordinal);
        private readonly IDictionary<JsonRpcId, JsonRpcResponseContract> _dynamicResponseContracts = new Dictionary<JsonRpcId, JsonRpcResponseContract>();
        private readonly IDictionary<JsonRpcId, string> _staticResponseBindings = new Dictionary<JsonRpcId, string>();

        /// <summary>Initializes a new instance of the <see cref="JsonRpcContractResolver" /> class.</summary>
        public JsonRpcContractResolver()
        {
        }

        JsonRpcRequestContract IJsonRpcContractResolver.GetRequestContract(string method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var lockTaken = false;

            _spinLock.Enter(ref lockTaken);
            _staticRequestContracts.TryGetValue(method, out var contract);

            if (lockTaken)
            {
                _spinLock.Exit();
            }

            return contract;
        }

        JsonRpcResponseContract IJsonRpcContractResolver.GetResponseContract(in JsonRpcId messageId)
        {
            var lockTaken = false;

            _spinLock.Enter(ref lockTaken);

            if (!_dynamicResponseContracts.TryGetValue(messageId, out var contract))
            {
                if (_staticResponseBindings.TryGetValue(messageId, out var method) && (method != null))
                {
                    _staticResponseContracts.TryGetValue(method, out contract);
                }
            }
            if (lockTaken)
            {
                _spinLock.Exit();
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

            var lockTaken = false;

            _spinLock.Enter(ref lockTaken);
            _staticRequestContracts[method] = contract;

            if (lockTaken)
            {
                _spinLock.Exit();
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

            var lockTaken = false;

            _spinLock.Enter(ref lockTaken);
            _staticResponseContracts[method] = contract;

            if (lockTaken)
            {
                _spinLock.Exit();
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

            var lockTaken = false;

            _spinLock.Enter(ref lockTaken);
            _dynamicResponseContracts[messageId] = contract;

            if (lockTaken)
            {
                _spinLock.Exit();
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

            var lockTaken = false;

            _spinLock.Enter(ref lockTaken);
            _staticResponseBindings[messageId] = method;

            if (lockTaken)
            {
                _spinLock.Exit();
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

            var lockTaken = false;

            _spinLock.Enter(ref lockTaken);
            _staticRequestContracts.Remove(method);

            if (lockTaken)
            {
                _spinLock.Exit();
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

            var lockTaken = false;

            _spinLock.Enter(ref lockTaken);
            _staticResponseContracts.Remove(method);

            if (lockTaken)
            {
                _spinLock.Exit();
            }
        }

        /// <summary>Removes the corresponding JSON-RPC response contract.</summary>
        /// <param name="messageId">The JSON-RPC message identifier.</param>
        public void RemoveResponseContract(in JsonRpcId messageId)
        {
            var lockTaken = false;

            _spinLock.Enter(ref lockTaken);
            _dynamicResponseContracts.Remove(messageId);

            if (lockTaken)
            {
                _spinLock.Exit();
            }
        }

        /// <summary>Removes the corresponding JSON-RPC response binding.</summary>
        /// <param name="messageId">The JSON-RPC message identifier.</param>
        public void RemoveResponseBinding(in JsonRpcId messageId)
        {
            var lockTaken = false;

            _spinLock.Enter(ref lockTaken);
            _staticResponseBindings.Remove(messageId);

            if (lockTaken)
            {
                _spinLock.Exit();
            }
        }

        /// <summary>Removes all JSON-RPC request contracts.</summary>
        public void ClearRequestContracts()
        {
            var lockTaken = false;

            _spinLock.Enter(ref lockTaken);
            _staticRequestContracts.Clear();

            if (lockTaken)
            {
                _spinLock.Exit();
            }
        }

        /// <summary>Removes all JSON-RPC response contracts.</summary>
        public void ClearResponseContracts()
        {
            var lockTaken = false;

            _spinLock.Enter(ref lockTaken);
            _staticResponseContracts.Clear();
            _dynamicResponseContracts.Clear();

            if (lockTaken)
            {
                _spinLock.Exit();
            }
        }

        /// <summary>Removes all JSON-RPC response bindings.</summary>
        public void ClearResponseBindings()
        {
            var lockTaken = false;

            _spinLock.Enter(ref lockTaken);
            _staticResponseBindings.Clear();

            if (lockTaken)
            {
                _spinLock.Exit();
            }
        }
    }
}