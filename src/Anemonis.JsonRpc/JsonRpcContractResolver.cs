﻿// © Alexander Kozlenko. Licensed under the MIT License.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Anemonis.JsonRpc
{
    /// <summary>Represents a JSON-RPC message contract resolver.</summary>
    public sealed class JsonRpcContractResolver : IJsonRpcContractResolver
    {
        private readonly SpinLock _operationLock = new(false);

        private readonly ConcurrentDictionary<string, JsonRpcRequestContract> _staticRequestContracts = new(StringComparer.Ordinal);
        private readonly ConcurrentDictionary<string, JsonRpcResponseContract> _staticResponseContracts = new(StringComparer.Ordinal);
        private readonly ConcurrentDictionary<JsonRpcId, JsonRpcResponseContract> _dynamicResponseContracts = new();
        private readonly ConcurrentDictionary<JsonRpcId, string> _staticResponseBindings = new();

        /// <summary>Initializes a new instance of the <see cref="JsonRpcContractResolver" /> class.</summary>
        public JsonRpcContractResolver()
        {
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="method" /> is <see langword="null" />.</exception>
        public JsonRpcRequestContract GetRequestContract(string method)
        {
            if (method is null)
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

        /// <inheritdoc />
        public JsonRpcResponseContract GetResponseContract(in JsonRpcId messageId)
        {
            var operationLockTaken = false;

            _operationLock.Enter(ref operationLockTaken);

            if (!_dynamicResponseContracts.TryGetValue(messageId, out var contract))
            {
                if (_staticResponseBindings.TryGetValue(messageId, out var method) && (method is not null))
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
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            if (contract is null)
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
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            if (contract is null)
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
            if (contract is null)
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
            if (method is null)
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
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var operationLockTaken = false;

            _operationLock.Enter(ref operationLockTaken);
            _staticRequestContracts.Remove(method, out var value);

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
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var operationLockTaken = false;

            _operationLock.Enter(ref operationLockTaken);
            _staticResponseContracts.Remove(method, out var value);

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
            _dynamicResponseContracts.Remove(messageId, out var value);

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
            _staticResponseBindings.Remove(messageId, out var value);

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
