// © Alexander Kozlenko. Licensed under the MIT License.

namespace Anemonis.JsonRpc
{
    internal static class HashCode
    {
        public const int FNV_PRIME_32 = 0x01000193;
        public const int FNV_OFFSET_BASIS_32 = unchecked((int)0x811C9DC5);
    }
}
