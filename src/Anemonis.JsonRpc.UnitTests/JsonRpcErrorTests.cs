using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable disable warnings

namespace Anemonis.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcErrorTests
    {
        [DataTestMethod]
        [DataRow(JsonRpcErrorCode.InvalidFormat)]
        [DataRow(JsonRpcErrorCode.InvalidOperation)]
        [DataRow(JsonRpcErrorCode.InvalidParameters)]
        [DataRow(JsonRpcErrorCode.InvalidMethod)]
        [DataRow(JsonRpcErrorCode.InvalidMessage)]
        [DataRow(JsonRpcErrorCode.ServerErrorsLowerBoundary)]
        [DataRow(JsonRpcErrorCode.ServerErrorsLowerBoundary + 1L)]
        [DataRow(JsonRpcErrorCode.ServerErrorsUpperBoundary - 1L)]
        [DataRow(JsonRpcErrorCode.ServerErrorsUpperBoundary)]
        [DataRow(JsonRpcErrorCode.SystemErrorsLowerBoundary - 1L)]
        [DataRow(JsonRpcErrorCode.SystemErrorsUpperBoundary + 1L)]
        [DataRow(default(long))]
        public void CodeIsValid(long code)
        {
            var jsonRpcError = new JsonRpcError(code, "m");

            Assert.AreEqual(code, jsonRpcError.Code);
        }

        [DataTestMethod]
        [DataRow(JsonRpcErrorCode.SystemErrorsLowerBoundary)]
        [DataRow(JsonRpcErrorCode.SystemErrorsLowerBoundary + 1L)]
        [DataRow(JsonRpcErrorCode.ServerErrorsLowerBoundary - 1L)]
        public void CodeIsInvalid(long code)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                new JsonRpcError(code, "m"));
        }

        [TestMethod]
        public void MessageIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new JsonRpcError(1L, null));
        }

        [TestMethod]
        public void MessageIsEmptyString()
        {
            var jsonRpcError = new JsonRpcError(1L, string.Empty);

            Assert.AreEqual(string.Empty, jsonRpcError.Message);
        }

        [TestMethod]
        public void HasDataIsFalse()
        {
            var jsonRpcError = new JsonRpcError(1L, "m");

            Assert.IsFalse(jsonRpcError.HasData);
        }

        [TestMethod]
        public void HasDataIsTrue()
        {
            var jsonRpcError = new JsonRpcError(1L, "m", null);

            Assert.IsTrue(jsonRpcError.HasData);
        }
    }
}
