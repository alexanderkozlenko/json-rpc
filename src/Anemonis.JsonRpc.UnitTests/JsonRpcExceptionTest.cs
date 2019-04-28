using System;

using Anemonis.JsonRpc.UnitTests.TestStubs;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anemonis.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcExceptionTest
    {
        [TestMethod]
        public void Constructor()
        {
            new TestJsonRpcException();
        }

        [TestMethod]
        public void ConstructorWithMessage()
        {
            new TestJsonRpcException("m");
        }

        [TestMethod]
        public void ConstructorWithMessageAndInnerException()
        {
            new TestJsonRpcException("m", new InvalidOperationException());
        }
    }
}
