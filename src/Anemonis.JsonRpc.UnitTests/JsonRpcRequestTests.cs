using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anemonis.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcRequestTests
    {
        [TestMethod]
        public void IsNotificationIsTrueWhenIdIsNone()
        {
            var message = new JsonRpcRequest("m");

            Assert.AreEqual(default, message.Id);
            Assert.IsTrue(message.IsNotification);
        }

        [TestMethod]
        public void IsNotificationIsFalseWhenIdIsNumber()
        {
            var message = new JsonRpcRequest("m", 1L);

            Assert.AreEqual(1L, message.Id);
            Assert.IsFalse(message.IsNotification);
        }

        [TestMethod]
        public void IsNotificationIsFalseWhenIdIsString()
        {
            var message = new JsonRpcRequest("m", "1");

            Assert.AreEqual("1", message.Id);
            Assert.IsFalse(message.IsNotification);
        }

        [TestMethod]
        public void ParametersTypeIsNoneWhenIdIsNone()
        {
            var message = new JsonRpcRequest("m");

            Assert.AreEqual(JsonRpcParametersType.None, message.ParametersType);
        }

        [TestMethod]
        public void ParametersTypeIsByPositionWhenIdIsNone()
        {
            var parameters = new object[] { 1L };
            var message = new JsonRpcRequest("m", parameters);

            Assert.AreEqual(JsonRpcParametersType.ByPosition, message.ParametersType);
        }

        [TestMethod]
        public void ParametersTypeIsByNameWhenIdIsNone()
        {
            var parameters = new Dictionary<string, object> { ["p"] = 1L };
            var message = new JsonRpcRequest("m", parameters);

            Assert.AreEqual(JsonRpcParametersType.ByName, message.ParametersType);
        }

        [TestMethod]
        public void IsSystemIsFalse()
        {
            var message = new JsonRpcRequest("m");

            Assert.IsFalse(message.IsSystem);
        }

        [TestMethod]
        public void IsSystemIsTrue()
        {
            var message = new JsonRpcRequest("rpc.m");

            Assert.IsTrue(message.IsSystem);
        }

        [TestMethod]
        public void MethodIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new JsonRpcRequest((string)null));
        }

        [TestMethod]
        public void MethodIsEmptyString()
        {
            var message = new JsonRpcRequest("");

            Assert.AreEqual("", message.Method);
        }
    }
}