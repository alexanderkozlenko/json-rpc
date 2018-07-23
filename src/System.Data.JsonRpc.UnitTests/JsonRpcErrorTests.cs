using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Data.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcErrorTests
    {
        [DataTestMethod]
        [DataRow(JsonRpcErrorCodes.StandardErrorsLowerBoundary - 1L)]
        [DataRow(JsonRpcErrorCodes.InvalidJson)]
        [DataRow(JsonRpcErrorCodes.InvalidOperation)]
        [DataRow(JsonRpcErrorCodes.InvalidParameters)]
        [DataRow(JsonRpcErrorCodes.InvalidMethod)]
        [DataRow(JsonRpcErrorCodes.InvalidMessage)]
        [DataRow(JsonRpcErrorCodes.ServerErrorsLowerBoundary)]
        [DataRow(JsonRpcErrorCodes.ServerErrorsLowerBoundary + 1L)]
        [DataRow(JsonRpcErrorCodes.ServerErrorsUpperBoundary - 1L)]
        [DataRow(JsonRpcErrorCodes.ServerErrorsUpperBoundary)]
        [DataRow(JsonRpcErrorCodes.StandardErrorsUpperBoundary + 1L)]
        [DataRow(default(long))]
        public void CodeIsValid(long code)
        {
            var jsonRpcError = new JsonRpcError(code, "m");

            Assert.AreEqual(code, jsonRpcError.Code);
        }

        [DataTestMethod]
        [DataRow(JsonRpcErrorCodes.StandardErrorsLowerBoundary)]
        [DataRow(JsonRpcErrorCodes.StandardErrorsLowerBoundary + 1L)]
        [DataRow(JsonRpcErrorCodes.ServerErrorsLowerBoundary - 1L)]
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