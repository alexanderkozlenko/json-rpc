using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Data.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcProtocolTests
    {
        [TestMethod]
        public void IsSystemMethodWhenMethodIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                JsonRpcProtocol.IsSystemMethod((string)null));
        }

        [TestMethod]
        public void IsSystemMethodIsFalse()
        {
            var result = JsonRpcProtocol.IsSystemMethod("m");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsSystemMethodIsTrueWithLowerCase()
        {
            var result = JsonRpcProtocol.IsSystemMethod("rpc.m");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsSystemMethodIsTrueWithUpperCase()
        {
            var result = JsonRpcProtocol.IsSystemMethod("RPC.M");

            Assert.IsTrue(result);
        }
    }
}