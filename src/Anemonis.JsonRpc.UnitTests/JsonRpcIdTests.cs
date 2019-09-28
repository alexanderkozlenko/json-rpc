using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anemonis.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcIdTests
    {
        [TestMethod]
        public void ConstructorWhenTypeIsStringAndValueIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new JsonRpcId(default(string)));
        }

        [TestMethod]
        public void ConstructorWhenTypeIsStringAndValueIsEmpty()
        {
            var value = new JsonRpcId(string.Empty);

            Assert.AreEqual(JsonRpcIdType.String, value.Type);
        }

        [TestMethod]
        public void ConstructorWhenTypeIsFloatAndValueIsNaN()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new JsonRpcId(double.NaN));
        }

        [TestMethod]
        public void ConstructorWhenTypeIsFloatAndValueIsNegativeInfinity()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new JsonRpcId(double.NegativeInfinity));
        }

        [TestMethod]
        public void ConstructorWhenTypeIsFloatAndValueIsPositiveInfinity()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new JsonRpcId(double.PositiveInfinity));
        }

        [TestMethod]
        public void TypeGet()
        {
            Assert.AreEqual(JsonRpcIdType.None, new JsonRpcId().Type);
            Assert.AreEqual(JsonRpcIdType.String, new JsonRpcId("1").Type);
            Assert.AreEqual(JsonRpcIdType.Number, new JsonRpcId(1D).Type);
        }

        [TestMethod]
        public void OperatorEquality()
        {
            Assert.IsTrue(
                new JsonRpcId() == new JsonRpcId());
            Assert.IsFalse(
                new JsonRpcId() == new JsonRpcId("1"));
            Assert.IsFalse(
                new JsonRpcId() == new JsonRpcId(1D));
            Assert.IsFalse(
                new JsonRpcId() == "1");
            Assert.IsFalse(
                new JsonRpcId() == 1D);

            Assert.IsFalse(
                new JsonRpcId("1") == new JsonRpcId());
            Assert.IsTrue(
                new JsonRpcId("1") == new JsonRpcId("1"));
            Assert.IsFalse(
                new JsonRpcId("1") == new JsonRpcId("2"));
            Assert.IsFalse(
                new JsonRpcId("1") == new JsonRpcId(1D));
            Assert.IsTrue(
                new JsonRpcId("1") == "1");
            Assert.IsFalse(
                new JsonRpcId("1") == "2");
            Assert.IsFalse(
                new JsonRpcId("1") == 1D);

            Assert.IsFalse(
                new JsonRpcId(1D) == new JsonRpcId());
            Assert.IsFalse(
                new JsonRpcId(1D) == new JsonRpcId("1"));
            Assert.IsTrue(
                new JsonRpcId(1D) == new JsonRpcId(1D));
            Assert.IsFalse(
                new JsonRpcId(1D) == new JsonRpcId(2D));
            Assert.IsFalse(
                new JsonRpcId(1D) == "1");
            Assert.IsTrue(
                new JsonRpcId(1D) == 1D);
            Assert.IsFalse(
                new JsonRpcId(1D) == 2D);
        }

        [TestMethod]
        public void OperatorInequality()
        {
            Assert.IsFalse(
                new JsonRpcId() != new JsonRpcId());
            Assert.IsTrue(
                new JsonRpcId() != new JsonRpcId("1"));
            Assert.IsTrue(
                new JsonRpcId() != new JsonRpcId(1D));
            Assert.IsTrue(
                new JsonRpcId() != "1");
            Assert.IsTrue(
                new JsonRpcId() != 1D);

            Assert.IsTrue(
                new JsonRpcId("1") != new JsonRpcId());
            Assert.IsFalse(
                new JsonRpcId("1") != new JsonRpcId("1"));
            Assert.IsTrue(
                new JsonRpcId("1") != new JsonRpcId("2"));
            Assert.IsTrue(
                new JsonRpcId("1") != new JsonRpcId(1D));
            Assert.IsFalse(
                new JsonRpcId("1") != "1");
            Assert.IsTrue(
                new JsonRpcId("1") != "2");
            Assert.IsTrue(
                new JsonRpcId("1") != 1D);

            Assert.IsTrue(
                new JsonRpcId(1D) != new JsonRpcId());
            Assert.IsTrue(
                new JsonRpcId(1D) != new JsonRpcId("1"));
            Assert.IsFalse(
                new JsonRpcId(1D) != new JsonRpcId(1D));
            Assert.IsTrue(
                new JsonRpcId(1D) != new JsonRpcId(2D));
            Assert.IsTrue(
                new JsonRpcId(1D) != "1");
            Assert.IsFalse(
                new JsonRpcId(1D) != 1D);
            Assert.IsTrue(
                new JsonRpcId(1D) != 2D);
        }

        [TestMethod]
        public void ObjectCastToString()
        {
            Assert.AreEqual(default, (string)new JsonRpcId());
            Assert.AreEqual("1", (string)new JsonRpcId("1"));
            Assert.AreEqual(default, (string)new JsonRpcId(1D));
        }

        [TestMethod]
        public void ObjectCastToDouble()
        {
            Assert.AreEqual(default, (double)new JsonRpcId());
            Assert.AreEqual(default, (double)new JsonRpcId("1"));
            Assert.AreEqual(1D, (double)new JsonRpcId(1D));
        }

        [TestMethod]
        public void ObjectEqualsWhenObject1IsDefaultJsonRpcId()
        {
            Assert.IsTrue(new JsonRpcId().Equals((object)new JsonRpcId()));
            Assert.IsFalse(new JsonRpcId("1").Equals((object)new JsonRpcId()));
            Assert.IsFalse(new JsonRpcId(1D).Equals((object)new JsonRpcId()));
        }

        [TestMethod]
        public void ObjectEqualsWhenObject1IsStringJsonRpcId()
        {
            Assert.IsFalse(new JsonRpcId().Equals((object)new JsonRpcId("1")));
            Assert.IsTrue(new JsonRpcId("1").Equals((object)new JsonRpcId("1")));
            Assert.IsFalse(new JsonRpcId("2").Equals((object)new JsonRpcId("1")));
            Assert.IsFalse(new JsonRpcId(1D).Equals((object)new JsonRpcId("1")));
        }

        [TestMethod]
        public void ObjectEqualsWhenObject1IsFloatJsonRpcId()
        {
            Assert.IsFalse(new JsonRpcId().Equals((object)new JsonRpcId(1D)));
            Assert.IsFalse(new JsonRpcId("1").Equals((object)new JsonRpcId(1D)));
            Assert.IsTrue(new JsonRpcId(1D).Equals((object)new JsonRpcId(1D)));
            Assert.IsFalse(new JsonRpcId(2D).Equals((object)new JsonRpcId(1D)));
        }

        [TestMethod]
        public void ObjectEqualsWhenObject1IsString()
        {
            Assert.IsFalse(new JsonRpcId().Equals((object)"1"));
            Assert.IsTrue(new JsonRpcId("1").Equals((object)"1"));
            Assert.IsFalse(new JsonRpcId("2").Equals((object)"1"));
            Assert.IsFalse(new JsonRpcId(1D).Equals((object)"1"));
        }

        [TestMethod]
        public void ObjectEqualsWhenObject1IsFloat()
        {
            Assert.IsFalse(new JsonRpcId().Equals((object)1D));
            Assert.IsFalse(new JsonRpcId("1").Equals((object)1D));
            Assert.IsTrue(new JsonRpcId(1D).Equals((object)1D));
            Assert.IsFalse(new JsonRpcId(2D).Equals((object)1D));
        }

        [TestMethod]
        public void ObjectEqualsWhenObject1IsNull()
        {
            Assert.IsTrue(new JsonRpcId().Equals((object)null));
            Assert.IsFalse(new JsonRpcId("1").Equals((object)null));
            Assert.IsFalse(new JsonRpcId(1D).Equals((object)null));
            Assert.IsFalse(new JsonRpcId(2D).Equals((object)null));
        }

        [TestMethod]
        public void ObjectGetHashCode()
        {
            Assert.AreEqual(new JsonRpcId().GetHashCode(), new JsonRpcId().GetHashCode());

            Assert.AreNotEqual(new JsonRpcId("1").GetHashCode(), new JsonRpcId().GetHashCode());
            Assert.AreEqual(new JsonRpcId("1").GetHashCode(), new JsonRpcId("1").GetHashCode());
            Assert.AreNotEqual(new JsonRpcId("1").GetHashCode(), new JsonRpcId("2").GetHashCode());
            Assert.AreNotEqual(new JsonRpcId("1").GetHashCode(), new JsonRpcId(1D).GetHashCode());

            Assert.AreNotEqual(new JsonRpcId(1D).GetHashCode(), new JsonRpcId().GetHashCode());
            Assert.AreNotEqual(new JsonRpcId(1D).GetHashCode(), new JsonRpcId("1").GetHashCode());
            Assert.AreEqual(new JsonRpcId(1D).GetHashCode(), new JsonRpcId(1D).GetHashCode());
            Assert.AreNotEqual(new JsonRpcId(1D).GetHashCode(), new JsonRpcId(2D).GetHashCode());
        }

        [TestMethod]
        public void ObjectToString()
        {
            Assert.IsNotNull(new JsonRpcId().ToString());
            Assert.IsNotNull(new JsonRpcId("1").ToString());
            Assert.IsNotNull(new JsonRpcId(1.1).ToString());
        }

        [TestMethod]
        public void ObjectToStringWithProviderWhenProviderIsNull()
        {
            Assert.IsNotNull(new JsonRpcId().ToString(null));
            Assert.IsNotNull(new JsonRpcId("1").ToString(null));
            Assert.IsNotNull(new JsonRpcId(1.1).ToString(null));
        }

        [TestMethod]
        public void StaticFromString()
        {
            var jsonRpcId = JsonRpcId.FromString("1");

            Assert.AreEqual(JsonRpcIdType.String, jsonRpcId.Type);
            Assert.AreEqual("1", jsonRpcId);
        }

        [TestMethod]
        public void StaticFromDouble()
        {
            var jsonRpcId = JsonRpcId.FromDouble(1D);

            Assert.AreEqual(JsonRpcIdType.Number, jsonRpcId.Type);
            Assert.AreEqual(1D, jsonRpcId);
        }

        [TestMethod]
        public void StaticToString()
        {
            var jsonRpcId = JsonRpcId.FromString("1");
            var value = JsonRpcId.ToString(jsonRpcId);

            Assert.AreEqual("1", value);
        }

        [TestMethod]
        public void StaticToDouble()
        {
            var jsonRpcId = JsonRpcId.FromDouble(1D);
            var value = JsonRpcId.ToDouble(jsonRpcId);

            Assert.AreEqual(1D, value);
        }
    }
}
