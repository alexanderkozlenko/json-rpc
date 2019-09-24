using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable disable warnings

namespace Anemonis.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcMessageInfoTests
    {
        [TestMethod]
        public void ObjectEqualsWhenObject1IsJsonRpcRequestAndObject2IsJsonRpcRequest()
        {
            Assert.IsTrue(default(JsonRpcMessageInfo<JsonRpcRequest>).Equals((object)default(JsonRpcMessageInfo<JsonRpcRequest>)));
        }

        [TestMethod]
        public void ObjectEqualsWhenObject1IsJsonRpcRequestAndObject2IsJsonRpcResponse()
        {
            Assert.IsFalse(default(JsonRpcMessageInfo<JsonRpcRequest>).Equals((object)default(JsonRpcMessageInfo<JsonRpcResponse>)));
        }

        [TestMethod]
        public void ObjectEqualsWhenObject1IsJsonRpcResponseAndObject2IsJsonRpcResponse()
        {
            Assert.IsTrue(default(JsonRpcMessageInfo<JsonRpcResponse>).Equals((object)default(JsonRpcMessageInfo<JsonRpcResponse>)));
        }

        [TestMethod]
        public void ObjectEqualsWhenObject1IsJsonRpcResponseAndObject2IsJsonRpcRequest()
        {
            Assert.IsFalse(default(JsonRpcMessageInfo<JsonRpcResponse>).Equals((object)default(JsonRpcMessageInfo<JsonRpcRequest>)));
        }

        [TestMethod]
        public void ObjectEqualsWhenObject1IsJsonRpcRequestAndObject2IsObject()
        {
            Assert.IsFalse(default(JsonRpcMessageInfo<JsonRpcRequest>).Equals(default(object)));
        }

        [TestMethod]
        public void ObjectEqualsWhenObject1IsJsonRpcResponseAndObject2IsObject()
        {
            Assert.IsFalse(default(JsonRpcMessageInfo<JsonRpcResponse>).Equals(default(object)));
        }

        [TestMethod]
        public void EqualityEqualsWhenObject1IsJsonRpcRequestAndObject2IsJsonRpcRequest()
        {
            Assert.IsTrue(default(JsonRpcMessageInfo<JsonRpcRequest>).Equals(default(JsonRpcMessageInfo<JsonRpcRequest>)));
        }

        [TestMethod]
        public void EqualityEqualsWhenObject1IsJsonRpcRequestAndObject2IsJsonRpcResponse()
        {
            Assert.IsFalse(default(JsonRpcMessageInfo<JsonRpcRequest>).Equals(default(JsonRpcMessageInfo<JsonRpcResponse>)));
        }

        [TestMethod]
        public void EqualityEqualsWhenObject1IsJsonRpcResponseAndObject2IsJsonRpcResponse()
        {
            Assert.IsTrue(default(JsonRpcMessageInfo<JsonRpcResponse>).Equals(default(JsonRpcMessageInfo<JsonRpcResponse>)));
        }

        [TestMethod]
        public void EqualityEqualsWhenObject1IsJsonRpcResponseAndObject2IsJsonRpcRequest()
        {
            Assert.IsFalse(default(JsonRpcMessageInfo<JsonRpcResponse>).Equals(default(JsonRpcMessageInfo<JsonRpcRequest>)));
        }

        [TestMethod]
        public void ObjectGetHashCodeWhenObject1IsJsonRpcRequestAndIstanceIsDefault()
        {
            Assert.AreNotEqual(0, default(JsonRpcMessageInfo<JsonRpcRequest>).GetHashCode());
        }

        [TestMethod]
        public void ObjectGetHashCodeWhenObject1IsJsonRpcResponseAndIstanceIsDefault()
        {
            Assert.AreNotEqual(0, default(JsonRpcMessageInfo<JsonRpcResponse>).GetHashCode());
        }

        [TestMethod]
        public void ObjectGetHashCodeWhenObject1IsJsonRpcRequest()
        {
            var jsonRpcMessageInfo = Create(new JsonRpcRequest(default, "m"));

            Assert.AreNotEqual(0, jsonRpcMessageInfo.GetHashCode());
        }

        [TestMethod]
        public void ObjectGetHashCodeWhenObject1IsJsonRpcResponse()
        {
            var jsonRpcMessageInfo = Create(new JsonRpcResponse("1", 1L));

            Assert.AreNotEqual(0, jsonRpcMessageInfo.GetHashCode());
        }

        [TestMethod]
        public void OperatorEqualityWhenObjectIsJsonRpcRequest()
        {
            Assert.IsTrue(default(JsonRpcMessageInfo<JsonRpcRequest>) == default(JsonRpcMessageInfo<JsonRpcRequest>));
        }

        [TestMethod]
        public void OperatorInequalityWhenObjectIsJsonRpcRequest()
        {
            Assert.IsFalse(default(JsonRpcMessageInfo<JsonRpcRequest>) != default(JsonRpcMessageInfo<JsonRpcRequest>));
        }

        [TestMethod]
        public void OperatorEqualityWhenObjectIsJsonRpcResponse()
        {
            Assert.IsTrue(default(JsonRpcMessageInfo<JsonRpcResponse>) == default(JsonRpcMessageInfo<JsonRpcResponse>));
        }

        [TestMethod]
        public void OperatorInequalityWhenObjectIsJsonRpcResponse()
        {
            Assert.IsFalse(default(JsonRpcMessageInfo<JsonRpcResponse>) != default(JsonRpcMessageInfo<JsonRpcResponse>));
        }

        private static JsonRpcMessageInfo<T> Create<T>(T value)
            where T : JsonRpcMessage
        {
            return Unsafe.As<T, JsonRpcMessageInfo<T>>(ref value);
        }
    }
}
