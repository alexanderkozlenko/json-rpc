using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Data.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcIdTests
    {
        [TestMethod]
        public void ConstructorWhenTypeIsStringAndValueIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(()
                => new JsonRpcId((string)null));
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
        public void IdTypeIsProper()
        {
            Assert.AreEqual(JsonRpcIdType.None, new JsonRpcId().Type);
            Assert.AreEqual(JsonRpcIdType.String, new JsonRpcId("1").Type);
            Assert.AreEqual(JsonRpcIdType.Integer, new JsonRpcId(1L).Type);
            Assert.AreEqual(JsonRpcIdType.Float, new JsonRpcId(1D).Type);
        }

        [TestMethod]
        public void OperatorEquality()
        {
            Assert.IsTrue(
                new JsonRpcId() == new JsonRpcId());
            Assert.IsFalse(
                new JsonRpcId() == new JsonRpcId("1"));
            Assert.IsFalse(
                new JsonRpcId() == new JsonRpcId(1L));
            Assert.IsFalse(
                new JsonRpcId() == new JsonRpcId(1D));
            Assert.IsFalse(
                new JsonRpcId() == "1");
            Assert.IsFalse(
                new JsonRpcId() == 1L);
            Assert.IsFalse(
                new JsonRpcId() == 1D);

            Assert.IsFalse(
                new JsonRpcId("1") == new JsonRpcId());
            Assert.IsTrue(
                new JsonRpcId("1") == new JsonRpcId("1"));
            Assert.IsFalse(
                new JsonRpcId("1") == new JsonRpcId("2"));
            Assert.IsFalse(
                new JsonRpcId("1") == new JsonRpcId(1L));
            Assert.IsFalse(
                new JsonRpcId("1") == new JsonRpcId(1D));
            Assert.IsTrue(
                new JsonRpcId("1") == "1");
            Assert.IsFalse(
                new JsonRpcId("1") == "2");
            Assert.IsFalse(
                new JsonRpcId("1") == 1L);
            Assert.IsFalse(
                new JsonRpcId("1") == 1D);

            Assert.IsFalse(
                new JsonRpcId(1L) == new JsonRpcId());
            Assert.IsFalse(
                new JsonRpcId(1L) == new JsonRpcId("1"));
            Assert.IsTrue(
                new JsonRpcId(1L) == new JsonRpcId(1L));
            Assert.IsFalse(
                new JsonRpcId(1L) == new JsonRpcId(2L));
            Assert.IsFalse(
                new JsonRpcId(1L) == new JsonRpcId(1D));
            Assert.IsFalse(
                new JsonRpcId(1L) == "1");
            Assert.IsTrue(
                new JsonRpcId(1L) == 1L);
            Assert.IsFalse(
                new JsonRpcId(1L) == 2L);
            Assert.IsFalse(
                new JsonRpcId(1L) == 1D);

            Assert.IsFalse(
                new JsonRpcId(1D) == new JsonRpcId());
            Assert.IsFalse(
                new JsonRpcId(1D) == new JsonRpcId("1"));
            Assert.IsFalse(
                new JsonRpcId(1D) == new JsonRpcId(1L));
            Assert.IsTrue(
                new JsonRpcId(1D) == new JsonRpcId(1D));
            Assert.IsFalse(
                new JsonRpcId(1D) == new JsonRpcId(2D));
            Assert.IsFalse(
                new JsonRpcId(1D) == "1");
            Assert.IsFalse(
                new JsonRpcId(1D) == 1L);
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
                new JsonRpcId() != new JsonRpcId(1L));
            Assert.IsTrue(
                new JsonRpcId() != new JsonRpcId(1D));
            Assert.IsTrue(
                new JsonRpcId() != "1");
            Assert.IsTrue(
                new JsonRpcId() != 1L);
            Assert.IsTrue(
                new JsonRpcId() != 1D);

            Assert.IsTrue(
                new JsonRpcId("1") != new JsonRpcId());
            Assert.IsFalse(
                new JsonRpcId("1") != new JsonRpcId("1"));
            Assert.IsTrue(
                new JsonRpcId("1") != new JsonRpcId("2"));
            Assert.IsTrue(
                new JsonRpcId("1") != new JsonRpcId(1L));
            Assert.IsTrue(
                new JsonRpcId("1") != new JsonRpcId(1D));
            Assert.IsFalse(
                new JsonRpcId("1") != "1");
            Assert.IsTrue(
                new JsonRpcId("1") != "2");
            Assert.IsTrue(
                new JsonRpcId("1") != 1L);
            Assert.IsTrue(
                new JsonRpcId("1") != 1D);

            Assert.IsTrue(
                new JsonRpcId(1L) != new JsonRpcId());
            Assert.IsTrue(
                new JsonRpcId(1L) != new JsonRpcId("1"));
            Assert.IsFalse(
                new JsonRpcId(1L) != new JsonRpcId(1L));
            Assert.IsTrue(
                new JsonRpcId(1L) != new JsonRpcId(2L));
            Assert.IsTrue(
                new JsonRpcId(1L) != new JsonRpcId(1D));
            Assert.IsTrue(
                new JsonRpcId(1L) != "1");
            Assert.IsFalse(
                new JsonRpcId(1L) != 1L);
            Assert.IsTrue(
                new JsonRpcId(1L) != 2L);
            Assert.IsTrue(
                new JsonRpcId(1L) != 1D);

            Assert.IsTrue(
                new JsonRpcId(1D) != new JsonRpcId());
            Assert.IsTrue(
                new JsonRpcId(1D) != new JsonRpcId("1"));
            Assert.IsTrue(
                new JsonRpcId(1D) != new JsonRpcId(1L));
            Assert.IsFalse(
                new JsonRpcId(1D) != new JsonRpcId(1D));
            Assert.IsTrue(
                new JsonRpcId(1D) != new JsonRpcId(2D));
            Assert.IsTrue(
                new JsonRpcId(1D) != "1");
            Assert.IsTrue(
                new JsonRpcId(1D) != 1L);
            Assert.IsFalse(
                new JsonRpcId(1D) != 1D);
            Assert.IsTrue(
                new JsonRpcId(1D) != 2D);
        }

        [TestMethod]
        public void ObjectCast()
        {
            Assert.ThrowsException<InvalidCastException>(() => (string)new JsonRpcId());
            Assert.AreEqual("1", (string)new JsonRpcId("1"));
            Assert.ThrowsException<InvalidCastException>(() => (string)new JsonRpcId(1L));
            Assert.ThrowsException<InvalidCastException>(() => (string)new JsonRpcId(1D));

            Assert.ThrowsException<InvalidCastException>(() => (long)new JsonRpcId());
            Assert.ThrowsException<InvalidCastException>(() => (long)new JsonRpcId("1"));
            Assert.AreEqual(1L, (long)new JsonRpcId(1L));
            Assert.ThrowsException<InvalidCastException>(() => (long)new JsonRpcId(1D));

            Assert.ThrowsException<InvalidCastException>(() => (double)new JsonRpcId());
            Assert.ThrowsException<InvalidCastException>(() => (double)new JsonRpcId("1"));
            Assert.ThrowsException<InvalidCastException>(() => (double)new JsonRpcId(1L));
            Assert.AreEqual(1D, (double)new JsonRpcId(1D));
        }

        [TestMethod]
        public void ObjectEquals()
        {
            Assert.IsTrue(
                object.Equals(new JsonRpcId(), new JsonRpcId()));
            Assert.IsFalse(
                object.Equals(new JsonRpcId(), new JsonRpcId("1")));
            Assert.IsFalse(
                object.Equals(new JsonRpcId(), new JsonRpcId(1L)));
            Assert.IsFalse(
                object.Equals(new JsonRpcId(), new JsonRpcId(1D)));

            Assert.IsFalse(
                object.Equals(new JsonRpcId("1"), new JsonRpcId()));
            Assert.IsTrue(
                object.Equals(new JsonRpcId("1"), new JsonRpcId("1")));
            Assert.IsFalse(
                object.Equals(new JsonRpcId("1"), new JsonRpcId("2")));
            Assert.IsFalse(
                object.Equals(new JsonRpcId("1"), new JsonRpcId(1L)));
            Assert.IsFalse(
                object.Equals(new JsonRpcId("1"), new JsonRpcId(1D)));

            Assert.IsFalse(
                object.Equals(new JsonRpcId(1L), new JsonRpcId()));
            Assert.IsFalse(
                object.Equals(new JsonRpcId(1L), new JsonRpcId("1")));
            Assert.IsTrue(
                object.Equals(new JsonRpcId(1L), new JsonRpcId(1L)));
            Assert.IsFalse(
                object.Equals(new JsonRpcId(1L), new JsonRpcId(2L)));
            Assert.IsFalse(
                object.Equals(new JsonRpcId(1L), new JsonRpcId(1D)));

            Assert.IsFalse(
                object.Equals(new JsonRpcId(1D), new JsonRpcId()));
            Assert.IsFalse(
                object.Equals(new JsonRpcId(1D), new JsonRpcId("1")));
            Assert.IsFalse(
                object.Equals(new JsonRpcId(1D), new JsonRpcId(1L)));
            Assert.IsTrue(
                object.Equals(new JsonRpcId(1D), new JsonRpcId(1D)));
            Assert.IsFalse(
                object.Equals(new JsonRpcId(1D), new JsonRpcId(2D)));
        }

        [TestMethod]
        public void ObjectGetHashCode()
        {
            Assert.AreEqual(
                new JsonRpcId().GetHashCode(), new JsonRpcId().GetHashCode());
            Assert.AreNotEqual(
                new JsonRpcId().GetHashCode(), new JsonRpcId("1").GetHashCode());
            Assert.AreNotEqual(
                new JsonRpcId().GetHashCode(), new JsonRpcId(1L).GetHashCode());
            Assert.AreNotEqual(
                new JsonRpcId().GetHashCode(), new JsonRpcId(1D).GetHashCode());

            Assert.AreNotEqual(
                new JsonRpcId("1").GetHashCode(), new JsonRpcId().GetHashCode());
            Assert.AreEqual(
                new JsonRpcId("1").GetHashCode(), new JsonRpcId("1").GetHashCode());
            Assert.AreNotEqual(
                new JsonRpcId("1").GetHashCode(), new JsonRpcId("2").GetHashCode());
            Assert.AreNotEqual(
                new JsonRpcId("1").GetHashCode(), new JsonRpcId(1L).GetHashCode());
            Assert.AreNotEqual(
                new JsonRpcId("1").GetHashCode(), new JsonRpcId(1D).GetHashCode());

            Assert.AreNotEqual(
                new JsonRpcId(1L).GetHashCode(), new JsonRpcId().GetHashCode());
            Assert.AreNotEqual(
                new JsonRpcId(1L).GetHashCode(), new JsonRpcId("1").GetHashCode());
            Assert.AreEqual(
                new JsonRpcId(1L).GetHashCode(), new JsonRpcId(1L).GetHashCode());
            Assert.AreNotEqual(
                new JsonRpcId(1L).GetHashCode(), new JsonRpcId(2L).GetHashCode());
            Assert.AreNotEqual(
                new JsonRpcId(1L).GetHashCode(), new JsonRpcId(1D).GetHashCode());

            Assert.AreNotEqual(
                new JsonRpcId(1D).GetHashCode(), new JsonRpcId().GetHashCode());
            Assert.AreNotEqual(
                new JsonRpcId(1D).GetHashCode(), new JsonRpcId("1").GetHashCode());
            Assert.AreNotEqual(
                new JsonRpcId(1D).GetHashCode(), new JsonRpcId(1L).GetHashCode());
            Assert.AreEqual(
                new JsonRpcId(1D).GetHashCode(), new JsonRpcId(1D).GetHashCode());
            Assert.AreNotEqual(
                new JsonRpcId(1D).GetHashCode(), new JsonRpcId(2D).GetHashCode());
        }

        [TestMethod]
        public void ObjectToString()
        {
            Assert.AreEqual("", new JsonRpcId().ToString(null));
            Assert.AreEqual("1", new JsonRpcId("1").ToString(null));
            Assert.AreEqual("1", new JsonRpcId(1L).ToString(null));
            Assert.AreEqual("1.1", new JsonRpcId(1.1).ToString(null));
        }

        [TestMethod]
        public void CompareTo()
        {
            Assert.AreEqual(+0, new JsonRpcId().CompareTo(new JsonRpcId()));
            Assert.AreEqual(-1, new JsonRpcId().CompareTo(new JsonRpcId("1")));
            Assert.AreEqual(-1, new JsonRpcId().CompareTo(new JsonRpcId(1L)));
            Assert.AreEqual(-1, new JsonRpcId().CompareTo(new JsonRpcId(1D)));

            Assert.AreEqual(+1, new JsonRpcId("1").CompareTo(new JsonRpcId()));
            Assert.AreEqual(+1, new JsonRpcId("1").CompareTo(new JsonRpcId("0")));
            Assert.AreEqual(+0, new JsonRpcId("1").CompareTo(new JsonRpcId("1")));
            Assert.AreEqual(-1, new JsonRpcId("1").CompareTo(new JsonRpcId("2")));
            Assert.AreEqual(-1, new JsonRpcId("1").CompareTo(new JsonRpcId(1L)));
            Assert.AreEqual(-1, new JsonRpcId("1").CompareTo(new JsonRpcId(1D)));

            Assert.AreEqual(+1, new JsonRpcId(1L).CompareTo(new JsonRpcId()));
            Assert.AreEqual(+1, new JsonRpcId(1L).CompareTo(new JsonRpcId("1")));
            Assert.AreEqual(+1, new JsonRpcId(1L).CompareTo(new JsonRpcId(0L)));
            Assert.AreEqual(+0, new JsonRpcId(1L).CompareTo(new JsonRpcId(1L)));
            Assert.AreEqual(-1, new JsonRpcId(1L).CompareTo(new JsonRpcId(2L)));
            Assert.AreEqual(-1, new JsonRpcId(1L).CompareTo(new JsonRpcId(1D)));

            Assert.AreEqual(+1, new JsonRpcId(1D).CompareTo(new JsonRpcId()));
            Assert.AreEqual(+1, new JsonRpcId(1D).CompareTo(new JsonRpcId("1")));
            Assert.AreEqual(+1, new JsonRpcId(1D).CompareTo(new JsonRpcId(0D)));
            Assert.AreEqual(+0, new JsonRpcId(1D).CompareTo(new JsonRpcId(1D)));
            Assert.AreEqual(-1, new JsonRpcId(1D).CompareTo(new JsonRpcId(2D)));
            Assert.AreEqual(+1, new JsonRpcId(1D).CompareTo(new JsonRpcId(1L)));
        }
    }
}