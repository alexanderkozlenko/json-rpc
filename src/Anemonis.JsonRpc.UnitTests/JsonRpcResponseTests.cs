using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable disable warnings

namespace Anemonis.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcResponseTests
    {
        [TestMethod]
        public void SuccessIsFalse()
        {
            var message = new JsonRpcResponse(1L, new JsonRpcError(2L, "m"));

            Assert.IsFalse(message.Success);
            Assert.IsNotNull(message.Error);
            Assert.IsNull(message.Result);
        }

        [TestMethod]
        public void SuccessIsTrueWhenResultIsNumber()
        {
            var message = new JsonRpcResponse(1L, 0L);

            Assert.IsTrue(message.Success);
            Assert.IsNull(message.Error);
        }

        [TestMethod]
        public void SuccessIsTrueWhenResultIsString()
        {
            var message = new JsonRpcResponse(1L, "0");

            Assert.IsTrue(message.Success);
            Assert.IsNull(message.Error);
        }

        [TestMethod]
        public void SuccessIsTrueWhenResultIsBoolean()
        {
            var message = new JsonRpcResponse(1L, true);

            Assert.IsTrue(message.Success);
            Assert.IsNull(message.Error);
        }

        [TestMethod]
        public void SuccessIsTrueWhenResultIsObject()
        {
            var message = new JsonRpcResponse(1L, new object());

            Assert.IsTrue(message.Success);
            Assert.IsNull(message.Error);
        }

        [TestMethod]
        public void SuccessIsTrueWhenResultIsNull()
        {
            var message = new JsonRpcResponse(1L, default(object));

            Assert.IsTrue(message.Success);
            Assert.IsNull(message.Error);
        }

        [TestMethod]
        public void SuccessIsTrueWhenIdIsNone()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new JsonRpcResponse(default, 1L));
        }

        [TestMethod]
        public void SuccessIsFalseWhenErrorIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new JsonRpcResponse(1L, null as JsonRpcError));
        }
    }
}
