using System;

namespace Anemonis.JsonRpc.UnitTests.TestStubs
{
    internal sealed class TestJsonRpcException : JsonRpcException
    {
        public TestJsonRpcException()
        {
        }

        public TestJsonRpcException(string message)
            : base(message)
        {
        }

        public TestJsonRpcException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
