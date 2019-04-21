using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anemonis.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcProtocolTests
    {
        [TestMethod]
        public void IsSystemMethodWhenMethodIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                JsonRpcProtocol.IsSystemMethod(null));
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

        [DataTestMethod]
        [DataRow(JsonRpcErrorCode.InvalidFormat)]
        [DataRow(JsonRpcErrorCode.InvalidOperation)]
        [DataRow(JsonRpcErrorCode.InvalidParameters)]
        [DataRow(JsonRpcErrorCode.InvalidMethod)]
        [DataRow(JsonRpcErrorCode.InvalidMessage)]
        [DataRow(JsonRpcErrorCode.ServerErrorsLowerBoundary)]
        [DataRow(JsonRpcErrorCode.ServerErrorsUpperBoundary)]
        [DataRow(JsonRpcErrorCode.SystemErrorsLowerBoundary)]
        [DataRow(JsonRpcErrorCode.SystemErrorsUpperBoundary)]
        public void IsSystemErrorCodeIsTrue(long code)
        {
            var result = JsonRpcProtocol.IsSystemErrorCode(code);

            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(JsonRpcErrorCode.SystemErrorsLowerBoundary - 1L)]
        [DataRow(JsonRpcErrorCode.SystemErrorsUpperBoundary + 1L)]
        [DataRow(0L)]
        public void IsSystemErrorCodeIsFalse(long code)
        {
            var result = JsonRpcProtocol.IsSystemErrorCode(code);

            Assert.IsFalse(result);
        }

        [DataTestMethod]
        [DataRow(JsonRpcErrorCode.ServerErrorsLowerBoundary)]
        [DataRow(JsonRpcErrorCode.ServerErrorsLowerBoundary + 1L)]
        [DataRow(JsonRpcErrorCode.ServerErrorsUpperBoundary)]
        [DataRow(JsonRpcErrorCode.ServerErrorsUpperBoundary - 1L)]
        public void IsServerErrorCodeIsTrue(long code)
        {
            var result = JsonRpcProtocol.IsServerErrorCode(code);

            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(JsonRpcErrorCode.InvalidFormat)]
        [DataRow(JsonRpcErrorCode.InvalidOperation)]
        [DataRow(JsonRpcErrorCode.InvalidParameters)]
        [DataRow(JsonRpcErrorCode.InvalidMethod)]
        [DataRow(JsonRpcErrorCode.InvalidMessage)]
        [DataRow(JsonRpcErrorCode.ServerErrorsLowerBoundary - 1L)]
        [DataRow(JsonRpcErrorCode.ServerErrorsUpperBoundary + 1L)]
        [DataRow(0L)]
        public void IsServerErrorCodeIsFalse(long code)
        {
            var result = JsonRpcProtocol.IsServerErrorCode(code);

            Assert.IsFalse(result);
        }

        [DataTestMethod]
        [DataRow(JsonRpcErrorCode.InvalidFormat)]
        [DataRow(JsonRpcErrorCode.InvalidOperation)]
        [DataRow(JsonRpcErrorCode.InvalidParameters)]
        [DataRow(JsonRpcErrorCode.InvalidMethod)]
        [DataRow(JsonRpcErrorCode.InvalidMessage)]
        public void IsStandardErrorCodeIsTrue(long code)
        {
            var result = JsonRpcProtocol.IsStandardErrorCode(code);

            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(JsonRpcErrorCode.ServerErrorsLowerBoundary)]
        [DataRow(JsonRpcErrorCode.ServerErrorsUpperBoundary)]
        [DataRow(JsonRpcErrorCode.SystemErrorsLowerBoundary)]
        [DataRow(JsonRpcErrorCode.SystemErrorsUpperBoundary)]
        [DataRow(0L)]
        public void IsStandardErrorCodeIsFalse(long code)
        {
            var result = JsonRpcProtocol.IsStandardErrorCode(code);

            Assert.IsFalse(result);
        }
    }
}