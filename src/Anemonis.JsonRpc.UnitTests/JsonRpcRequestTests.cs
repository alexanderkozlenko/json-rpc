using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anemonis.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcRequestTests
    {
        [TestMethod]
        public void ConstructorWhenMethodIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new JsonRpcRequest(default, null));
        }

        [TestMethod]
        public void ConstructorWithIdAndMethodAndParametersByPositionWhenParametersIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new JsonRpcRequest(1L, "m", (IReadOnlyList<object>)null));
        }

        [TestMethod]
        public void ConstructorWithIdAndMethodAndParametersByNameWhenParametersIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new JsonRpcRequest(1L, "m", (IReadOnlyDictionary<string, object>)null));
        }

        [TestMethod]
        public void IsNotificationIsTrueWhenIdIsNone()
        {
            var message = new JsonRpcRequest(default, "m");

            Assert.AreEqual(default, message.Id);
            Assert.IsTrue(message.IsNotification);
        }

        [TestMethod]
        public void IsNotificationIsFalseWhenIdIsNumber()
        {
            var message = new JsonRpcRequest(1L, "m");

            Assert.AreEqual(1L, message.Id);
            Assert.IsFalse(message.IsNotification);
        }

        [TestMethod]
        public void IsNotificationIsFalseWhenIdIsString()
        {
            var message = new JsonRpcRequest("1", "m");

            Assert.AreEqual("1", message.Id);
            Assert.IsFalse(message.IsNotification);
        }

        [TestMethod]
        public void ParametersTypeIsNoneWhenIdIsNone()
        {
            var message = new JsonRpcRequest(default, "m");

            Assert.AreEqual(JsonRpcParametersType.None, message.ParametersType);
        }

        [TestMethod]
        public void ParametersTypeIsByPositionWhenIdIsNone()
        {
            var parameters = new object[] { 1L };
            var message = new JsonRpcRequest(default, "m", parameters);

            Assert.AreEqual(JsonRpcParametersType.ByPosition, message.ParametersType);
        }

        [TestMethod]
        public void ParametersTypeIsByNameWhenIdIsNone()
        {
            var parameters = new Dictionary<string, object> { ["p"] = 1L };
            var message = new JsonRpcRequest(default, "m", parameters);

            Assert.AreEqual(JsonRpcParametersType.ByName, message.ParametersType);
        }

        [TestMethod]
        public void IsSystemIsFalse()
        {
            var message = new JsonRpcRequest(default, "m");

            Assert.IsFalse(message.IsSystem);
        }

        [TestMethod]
        public void IsSystemIsTrue()
        {
            var message = new JsonRpcRequest(default, "rpc.m");

            Assert.IsTrue(message.IsSystem);
        }

        [TestMethod]
        public void MethodIsEmptyString()
        {
            var message = new JsonRpcRequest(default, "");

            Assert.AreEqual("", message.Method);
        }
    }
}
