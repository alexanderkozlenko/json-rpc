using System.Linq;

using Anemonis.Resources;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anemonis.JsonRpc.SystemTests
{
    [TestClass]
    public sealed class JsonRpcSerializerTestsV1
    {
        #region Example T01: Echo service

        [TestMethod]
        public void DeserializeRequestDataT010()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_spec_01.0_req.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jrcr.AddRequestContract("echo", new JsonRpcRequestContract(new[] { typeof(string) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(1L, jrm.Id);
            Assert.AreEqual("echo", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm.ParametersType);

            CollectionAssert.AreEqual(new object[] { "Hello JSON-RPC" }, jrm.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void SerializeRequestT010()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_spec_01.0_req.json");
            var jrs = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);
            var jrm = new JsonRpcRequest(1L, "echo", new object[] { "Hello JSON-RPC" });
            var jsonr = jrs.SerializeRequest(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        [TestMethod]
        public void DeserializeResponseDataT010()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_spec_01.0_res.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jrcr.AddResponseContract("echo", new JsonRpcResponseContract(typeof(string)));
            jrcr.AddResponseBinding(1L, "echo");

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(1L, jrm.Id);
            Assert.IsInstanceOfType(jrm.Result, typeof(string));
            Assert.AreEqual("Hello JSON-RPC", jrm.Result);
        }

        [TestMethod]
        public void SerializeResponseT010()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_spec_01.0_res.json");
            var jrs = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);
            var jrm = new JsonRpcResponse(1L, "Hello JSON-RPC");
            var jsonr = jrs.SerializeResponse(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        #endregion

        #region Example T02: Chat application

        [TestMethod]
        public void DeserializeRequestDataT020()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_spec_02.0_req.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jrcr.AddRequestContract("postMessage", new JsonRpcRequestContract(new[] { typeof(string) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(99L, jrm.Id);
            Assert.AreEqual("postMessage", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm.ParametersType);

            CollectionAssert.AreEqual(new object[] { "Hello all!" }, jrm.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void SerializeRequestT020()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_spec_02.0_req.json");
            var jrs = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);
            var jrm = new JsonRpcRequest(99L, "postMessage", new object[] { "Hello all!" });
            var jsonr = jrs.SerializeRequest(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        [TestMethod]
        public void DeserializeResponseDataT020()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_spec_02.0_res.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jrcr.AddResponseContract("echo", new JsonRpcResponseContract(typeof(long)));
            jrcr.AddResponseBinding(99L, "echo");

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(99L, jrm.Id);
            Assert.IsInstanceOfType(jrm.Result, typeof(long));
            Assert.AreEqual(1L, jrm.Result);
        }

        [TestMethod]
        public void SerializeResponseT020()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_spec_02.0_res.json");
            var jrs = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);
            var jrm = new JsonRpcResponse(99L, 1L);
            var jsonr = jrs.SerializeResponse(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        [TestMethod]
        public void DeserializeRequestDataT021()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_spec_02.1_req.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jrcr.AddRequestContract("handleMessage", new JsonRpcRequestContract(new[] { typeof(string), typeof(string) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(default, jrm.Id);
            Assert.AreEqual("handleMessage", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm.ParametersType);

            CollectionAssert.AreEqual(new object[] { "user1", "we were just talking" }, jrm.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void SerializeRequestT021()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_spec_02.1_req.json");
            var jrs = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);
            var jrm = new JsonRpcRequest(default, "handleMessage", new object[] { "user1", "we were just talking" });
            var jsonr = jrs.SerializeRequest(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        [TestMethod]
        public void DeserializeRequestDataT022()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_spec_02.2_req.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jrcr.AddRequestContract("handleMessage", new JsonRpcRequestContract(new[] { typeof(string), typeof(string) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(default, jrm.Id);
            Assert.AreEqual("handleMessage", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm.ParametersType);

            CollectionAssert.AreEqual(new object[] { "user3", "sorry, gotta go now, ttyl" }, jrm.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void SerializeRequestT022()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_spec_02.2_req.json");
            var jrs = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);
            var jrm = new JsonRpcRequest(default, "handleMessage", new object[] { "user3", "sorry, gotta go now, ttyl" });
            var jsonr = jrs.SerializeRequest(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        [TestMethod]
        public void DeserializeRequestDataT023()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_spec_02.3_req.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jrcr.AddRequestContract("postMessage", new JsonRpcRequestContract(new[] { typeof(string) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(101L, jrm.Id);
            Assert.AreEqual("postMessage", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm.ParametersType);

            CollectionAssert.AreEqual(new object[] { "I have a question:" }, jrm.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void SerializeRequestT023()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_spec_02.3_req.json");
            var jrs = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);
            var jrm = new JsonRpcRequest(101L, "postMessage", new object[] { "I have a question:" });
            var jsonr = jrs.SerializeRequest(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        [TestMethod]
        public void DeserializeRequestDataT024()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_spec_02.4_req.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jrcr.AddRequestContract("userLeft", new JsonRpcRequestContract(new[] { typeof(string) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(default, jrm.Id);
            Assert.AreEqual("userLeft", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm.ParametersType);

            CollectionAssert.AreEqual(new object[] { "user3" }, jrm.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void SerializeRequestT024()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_spec_02.4_req.json");
            var jrs = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);
            var jrm = new JsonRpcRequest(default, "userLeft", new object[] { "user3" });
            var jsonr = jrs.SerializeRequest(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        [TestMethod]
        public void DeserializeResponseDataT025()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_spec_02.5_res.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jrcr.AddResponseContract("postMessage", new JsonRpcResponseContract(typeof(long)));
            jrcr.AddResponseBinding(101L, "postMessage");

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsFalse(jrd.IsBatch);

            var jrmi = jrd.Item;

            Assert.IsTrue(jrmi.IsValid);

            var jrm = jrmi.Message;

            Assert.AreEqual(101L, jrm.Id);
            Assert.IsInstanceOfType(jrm.Result, typeof(long));
            Assert.AreEqual(1L, jrm.Result);
        }

        [TestMethod]
        public void SerializeResponseT025()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v1_spec_02.5_res.json");
            var jrs = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);
            var jrm = new JsonRpcResponse(101L, 1L);
            var jsonr = jrs.SerializeResponse(jrm);

            JsonRpcSerializerTests.CompareJsonStrings(jsont, jsonr);
        }

        #endregion
    }
}
