// © Alexander Kozlenko. Licensed under the MIT License.

using System.Buffers;

using Newtonsoft.Json;

namespace Anemonis.JsonRpc
{
    internal sealed class JsonBufferPool : IArrayPool<char>
    {
        public char[] Rent(int minimumLength)
        {
            return ArrayPool<char>.Shared.Rent(minimumLength);
        }

        public void Return(char[] array)
        {
            ArrayPool<char>.Shared.Return(array);
        }
    }
}
