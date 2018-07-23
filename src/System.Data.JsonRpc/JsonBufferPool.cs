// © Alexander Kozlenko. Licensed under the MIT License.

using System.Buffers;
using Newtonsoft.Json;

namespace System.Data.JsonRpc
{
    internal sealed class JsonBufferPool : IArrayPool<char>
    {
        private static readonly ArrayPool<char> _arrayPool = ArrayPool<char>.Shared;

        public char[] Rent(int minimumLength)
        {
            return _arrayPool.Rent(minimumLength);
        }

        public void Return(char[] array)
        {
            _arrayPool.Return(array);
        }
    }
}