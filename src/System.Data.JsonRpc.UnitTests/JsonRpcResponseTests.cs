using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Data.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcResponseTests
    {
        [TestMethod]
        public void SuccessIsFalse()
        {
            var message = new JsonRpcResponse(new JsonRpcError(2L, "m"), 1L);

            Assert.IsFalse(message.Success);
            Assert.IsNotNull(message.Error);
            Assert.IsNull(message.Result);
        }

        [TestMethod]
        public void SuccessIsTrueWhenResultIsNumber()
        {
            var message = new JsonRpcResponse(0L, 1L);

            Assert.IsTrue(message.Success);
            Assert.IsNull(message.Error);
        }

        [TestMethod]
        public void SuccessIsTrueWhenResultIsString()
        {
            var message = new JsonRpcResponse("0", 1L);

            Assert.IsTrue(message.Success);
            Assert.IsNull(message.Error);
        }

        [TestMethod]
        public void SuccessIsTrueWhenResultIsBoolean()
        {
            var message = new JsonRpcResponse(true, 1L);

            Assert.IsTrue(message.Success);
            Assert.IsNull(message.Error);
        }

        [TestMethod]
        public void SuccessIsTrueWhenResultIsObject()
        {
            var message = new JsonRpcResponse(new object(), 1L);

            Assert.IsTrue(message.Success);
            Assert.IsNull(message.Error);
        }

        [TestMethod]
        public void SuccessIsTrueWhenResultIsNull()
        {
            var message = new JsonRpcResponse(default(object), 1L);

            Assert.IsTrue(message.Success);
            Assert.IsNull(message.Error);
        }

        [TestMethod]
        public void SuccessIsTrueWhenIdIsNone()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new JsonRpcResponse(1L, default(JsonRpcId)));
        }

        [TestMethod]
        public void SuccessIsFalseWhenErrorIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new JsonRpcResponse(default(JsonRpcError), 1L));
        }
    }
}