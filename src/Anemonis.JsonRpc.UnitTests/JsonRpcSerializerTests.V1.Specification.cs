using System.Linq;
using Anemonis.JsonRpc.UnitTests.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anemonis.JsonRpc.UnitTests
{
    public partial class JsonRpcSerializerTests
    {
        // Tests based on the JSON-RPC 1.0 specification (http://www.jsonrpc.org/specification_v1)

        #region Example V2 T01: Echo service

        [TestMethod]
        public void V1SpecT010DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_spec_01.0_req.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jsonRpcContractResolver.AddRequestContract("echo", new JsonRpcRequestContract(new[] { typeof(string) }));

            var jsonRpcData = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcMessageInfo = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(1L, jsonRpcMessage.Id);
            Assert.AreEqual("echo", jsonRpcMessage.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcMessage.ParametersType);

            CollectionAssert.AreEqual(new object[] { "Hello JSON-RPC" }, jsonRpcMessage.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void V1SpecT010SerializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_spec_01.0_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            var jsonRpcMessage = new JsonRpcRequest("echo", 1L, new object[] { "Hello JSON-RPC" });
            var jsonResult = jsonRpcSerializer.SerializeRequest(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V1SpecT010DeserializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_spec_01.0_res.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jsonRpcContractResolver.AddResponseContract("echo", new JsonRpcResponseContract(typeof(string)));
            jsonRpcContractResolver.AddResponseBinding(1L, "echo");

            var jsonRpcData = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcMessageInfo = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(1L, jsonRpcMessage.Id);
            Assert.IsInstanceOfType(jsonRpcMessage.Result, typeof(string));
            Assert.AreEqual("Hello JSON-RPC", jsonRpcMessage.Result);
        }

        [TestMethod]
        public void V1SpecT010SerializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_spec_01.0_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);
            var jsonRpcMessage = new JsonRpcResponse("Hello JSON-RPC", 1L);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        #endregion

        #region Example V2 T02: Chat application

        [TestMethod]
        public void V1SpecT020DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_spec_02.0_req.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jsonRpcContractResolver.AddRequestContract("postMessage", new JsonRpcRequestContract(new[] { typeof(string) }));

            var jsonRpcData = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcMessageInfo = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(99L, jsonRpcMessage.Id);
            Assert.AreEqual("postMessage", jsonRpcMessage.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcMessage.ParametersType);

            CollectionAssert.AreEqual(new object[] { "Hello all!" }, jsonRpcMessage.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void V1SpecT020SerializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_spec_02.0_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);
            var jsonRpcMessage = new JsonRpcRequest("postMessage", 99L, new object[] { "Hello all!" });
            var jsonResult = jsonRpcSerializer.SerializeRequest(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V1SpecT020DeserializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_spec_02.0_res.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jsonRpcContractResolver.AddResponseContract("echo", new JsonRpcResponseContract(typeof(long)));
            jsonRpcContractResolver.AddResponseBinding(99L, "echo");

            var jsonRpcData = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcMessageInfo = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(99L, jsonRpcMessage.Id);
            Assert.IsInstanceOfType(jsonRpcMessage.Result, typeof(long));
            Assert.AreEqual(1L, jsonRpcMessage.Result);
        }

        [TestMethod]
        public void V1SpecT020SerializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_spec_02.0_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            var jsonRpcMessage = new JsonRpcResponse(1L, 99L);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V1SpecT021DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_spec_02.1_req.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jsonRpcContractResolver.AddRequestContract("handleMessage", new JsonRpcRequestContract(new[] { typeof(string), typeof(string) }));

            var jsonRpcData = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcMessageInfo = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(default, jsonRpcMessage.Id);
            Assert.AreEqual("handleMessage", jsonRpcMessage.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcMessage.ParametersType);

            CollectionAssert.AreEqual(new object[] { "user1", "we were just talking" }, jsonRpcMessage.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void V1SpecT021SerializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_spec_02.1_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);
            var jsonRpcMessage = new JsonRpcRequest("handleMessage", new object[] { "user1", "we were just talking" });
            var jsonResult = jsonRpcSerializer.SerializeRequest(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V1SpecT021DeserializeResponse()
        {
            // N/A
        }

        [TestMethod]
        public void V1SpecT021SerializeResponse()
        {
            // N/A
        }

        [TestMethod]
        public void V1SpecT022DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_spec_02.2_req.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jsonRpcContractResolver.AddRequestContract("handleMessage", new JsonRpcRequestContract(new[] { typeof(string), typeof(string) }));

            var jsonRpcData = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcMessageInfo = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(default, jsonRpcMessage.Id);
            Assert.AreEqual("handleMessage", jsonRpcMessage.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcMessage.ParametersType);

            CollectionAssert.AreEqual(new object[] { "user3", "sorry, gotta go now, ttyl" }, jsonRpcMessage.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void V1SpecT022SerializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_spec_02.2_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);
            var jsonRpcMessage = new JsonRpcRequest("handleMessage", new object[] { "user3", "sorry, gotta go now, ttyl" });
            var jsonResult = jsonRpcSerializer.SerializeRequest(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V1SpecT022DeserializeResponse()
        {
            // N/A
        }

        [TestMethod]
        public void V1SpecT022SerializeResponse()
        {
            // N/A
        }

        [TestMethod]
        public void V1SpecT023DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_spec_02.3_req.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jsonRpcContractResolver.AddRequestContract("postMessage", new JsonRpcRequestContract(new[] { typeof(string) }));

            var jsonRpcData = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcMessageInfo = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(101L, jsonRpcMessage.Id);
            Assert.AreEqual("postMessage", jsonRpcMessage.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcMessage.ParametersType);

            CollectionAssert.AreEqual(new object[] { "I have a question:" }, jsonRpcMessage.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void V1SpecT023SerializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_spec_02.3_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);
            var jsonRpcMessage = new JsonRpcRequest("postMessage", 101L, new object[] { "I have a question:" });
            var jsonResult = jsonRpcSerializer.SerializeRequest(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V1SpecT023DeserializeResponse()
        {
            // N/A
        }

        [TestMethod]
        public void V1SpecT023SerializeResponse()
        {
            // N/A
        }

        [TestMethod]
        public void V1SpecT024DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_spec_02.4_req.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jsonRpcContractResolver.AddRequestContract("userLeft", new JsonRpcRequestContract(new[] { typeof(string) }));

            var jsonRpcData = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcMessageInfo = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(default, jsonRpcMessage.Id);
            Assert.AreEqual("userLeft", jsonRpcMessage.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcMessage.ParametersType);

            CollectionAssert.AreEqual(new object[] { "user3" }, jsonRpcMessage.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void V1SpecT024SerializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_spec_02.4_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);
            var jsonRpcMessage = new JsonRpcRequest("userLeft", new object[] { "user3" });
            var jsonResult = jsonRpcSerializer.SerializeRequest(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V1SpecT024DeserializeResponse()
        {
            // N/A
        }

        [TestMethod]
        public void V1SpecT024SerializeResponse()
        {
            // N/A
        }

        [TestMethod]
        public void V1SpecT025DeserializeRequest()
        {
            // N/A
        }

        [TestMethod]
        public void V1SpecT025SerializeRequest()
        {
            // N/A
        }

        [TestMethod]
        public void V1SpecT025DeserializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_spec_02.5_res.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jsonRpcContractResolver.AddResponseContract("postMessage", new JsonRpcResponseContract(typeof(long)));
            jsonRpcContractResolver.AddResponseBinding(101L, "postMessage");

            var jsonRpcData = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcMessageInfo = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual(101L, jsonRpcMessage.Id);
            Assert.IsInstanceOfType(jsonRpcMessage.Result, typeof(long));
            Assert.AreEqual(1L, jsonRpcMessage.Result);
        }

        [TestMethod]
        public void V1SpecT025SerializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_spec_02.5_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);
            var jsonRpcMessage = new JsonRpcResponse(1L, 101L);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        #endregion
    }
}