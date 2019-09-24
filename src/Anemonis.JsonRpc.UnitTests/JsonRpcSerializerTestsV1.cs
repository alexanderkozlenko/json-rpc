using System.Collections.Generic;

using Anemonis.JsonRpc.UnitTests.Resources;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable disable warnings

namespace Anemonis.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcSerializerTestsV1
    {
        [TestMethod]
        public void SerializeRequestWhenParametersAreByName()
        {
            var jrs = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);
            var jrmp = new Dictionary<string, object> { ["p"] = 1L };
            var jrm = new JsonRpcRequest(default, "m", jrmp);

            var jre = Assert.ThrowsException<JsonRpcSerializationException>(() =>
                jrs.SerializeRequest(jrm));

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre.ErrorCode);
        }

        [TestMethod]
        public void DeserializeResponseDataTIITB0I0E1D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_tc_it_res_b0i0e1d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jrcr.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jrcr.AddResponseBinding(1L, "m");

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.IsFalse(jrm.Success);
            Assert.AreEqual(default, jrm.Id);
            Assert.IsNull(jrm.Result);
            Assert.IsNotNull(jrm.Error);

            var jre = jrm.Error;

            Assert.AreEqual(default, jre.Code);
            Assert.AreEqual(string.Empty, jre.Message);
            Assert.IsFalse(jre.HasData);
        }

        [TestMethod]
        public void DeserializeResponseDataTIISB0I0E1D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_tc_is_res_b0i0e1d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jrcr.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jrcr.AddResponseBinding(1L, "m");

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsFalse(jrmi.IsValid);

            var jre = jrmi.Exception;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre.ErrorCode);
        }

        [TestMethod]
        public void DeserializeResponseDataTIISB0I0E0D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_tc_is_res_b0i0e0d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jrcr.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jrcr.AddResponseBinding(1L, "m");

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsFalse(jrmi.IsValid);

            var jre = jrmi.Exception;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre.ErrorCode);
        }
    }
}
