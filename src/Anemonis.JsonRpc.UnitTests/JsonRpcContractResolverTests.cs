using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anemonis.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcContractResolverTests
    {
        [TestMethod]
        public void AddRequestContractWhenMethodIsNull()
        {
            var resolver = new JsonRpcContractResolver();

            Assert.ThrowsException<ArgumentNullException>(() =>
                resolver.AddRequestContract(null, new JsonRpcRequestContract()));
        }

        [TestMethod]
        public void AddRequestContractWhenContractIsNull()
        {
            var resolver = new JsonRpcContractResolver();

            Assert.ThrowsException<ArgumentNullException>(() =>
                resolver.AddRequestContract("m", null));
        }

        [TestMethod]
        public void AddResponseContractWithMethodWhentMethodIsNull()
        {
            var resolver = new JsonRpcContractResolver();

            Assert.ThrowsException<ArgumentNullException>(() =>
                resolver.AddResponseContract(null, new JsonRpcResponseContract(null)));
        }

        [TestMethod]
        public void AddResponseContractWithMethodWhenContractIsNull()
        {
            var resolver = new JsonRpcContractResolver();

            Assert.ThrowsException<ArgumentNullException>(() =>
                resolver.AddResponseContract("m", null));
        }

        [TestMethod]
        public void AddResponseContractWithIdentifierWhenContractIsNull()
        {
            var resolver = new JsonRpcContractResolver();

            Assert.ThrowsException<ArgumentNullException>(() =>
                resolver.AddResponseContract(new JsonRpcId(0L), null));
        }

        [TestMethod]
        public void AddResponseBindingWhenMethodIsNull()
        {
            var resolver = new JsonRpcContractResolver();

            Assert.ThrowsException<ArgumentNullException>(() =>
                resolver.AddResponseBinding(0L, null));
        }

        [TestMethod]
        public void RemoveRequestContractWhenMethodIsNull()
        {
            var resolver = new JsonRpcContractResolver();

            Assert.ThrowsException<ArgumentNullException>(() =>
                resolver.RemoveRequestContract(null));
        }

        [TestMethod]
        public void RemoveResponseContractWithMethodWhenMethodIsNull()
        {
            var resolver = new JsonRpcContractResolver();

            Assert.ThrowsException<ArgumentNullException>(() =>
                resolver.RemoveResponseContract(null));
        }

        [TestMethod]
        public void GetRequestContractWhenMethodIsNull()
        {
            var resolver = new JsonRpcContractResolver() as IJsonRpcContractResolver;

            Assert.ThrowsException<ArgumentNullException>(() =>
                resolver.GetRequestContract(null));
        }
    }
}