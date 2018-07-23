using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Data.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcRequestContractTests
    {
        [TestMethod]
        public void ParametersTypeIsByPositionWhenIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new JsonRpcRequestContract(default(IReadOnlyList<Type>)));
        }

        [TestMethod]
        public void ParametersTypeIsByNameWhenIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new JsonRpcRequestContract(default(IReadOnlyDictionary<string, Type>)));
        }

        [TestMethod]
        public void ParametersTypeIsNone()
        {
            var contract = new JsonRpcRequestContract();

            Assert.AreEqual(JsonRpcParametersType.None, contract.ParametersType);
        }

        [TestMethod]
        public void ParametersTypeIsByPosition()
        {
            var parameters = new[] { typeof(long) };
            var contract = new JsonRpcRequestContract(parameters);

            Assert.AreEqual(JsonRpcParametersType.ByPosition, contract.ParametersType);
        }

        [TestMethod]
        public void ParametersTypeIsByName()
        {
            var parameters = new Dictionary<string, Type> { ["p"] = typeof(long) };
            var contract = new JsonRpcRequestContract(parameters);

            Assert.AreEqual(JsonRpcParametersType.ByName, contract.ParametersType);
        }
    }
}