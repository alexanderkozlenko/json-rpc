using System.Collections.Generic;
using System.Linq;
using Anemonis.JsonRpc.UnitTests.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anemonis.JsonRpc.UnitTests
{
    public partial class JsonRpcSerializerTests
    {
        // Tests based on the Bitcoin protocol specification (https://bitcoin.org/en/developer-reference#remote-procedure-calls-rpcs)

        #region Example V1 T01: Bitcoin method "getblockhash" with successful result

        [TestMethod]
        public void V1BitcoinT01DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_btc_01_req.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jsonRpcContractResolver.AddRequestContract("getblockhash", new JsonRpcRequestContract(new[] { typeof(long) }));

            var jsonRpcData = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcMessageInfo = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual("foo", jsonRpcMessage.Id);
            Assert.AreEqual("getblockhash", jsonRpcMessage.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcMessage.ParametersType);

            CollectionAssert.AreEqual(new object[] { 0L }, jsonRpcMessage.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void V1BitcoinT01SerializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_btc_01_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            var jsonRpcMessage = new JsonRpcRequest("foo", "getblockhash", new object[] { 0L });
            var jsonResult = jsonRpcSerializer.SerializeRequest(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V1BitcoinT01DeserializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_btc_01_res.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jsonRpcContractResolver.AddResponseContract("getblockhash", new JsonRpcResponseContract(typeof(string)));
            jsonRpcContractResolver.AddResponseBinding("foo", "getblockhash");

            var jsonRpcData = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcMessageInfo = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual("foo", jsonRpcMessage.Id);
            Assert.IsInstanceOfType(jsonRpcMessage.Result, typeof(string));
            Assert.AreEqual("000000000019d6689c085ae165831e934ff763ae46a2a6c172b3f1b60a8ce26f", jsonRpcMessage.Result);
        }

        [TestMethod]
        public void V1BitcoinT01SerializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_btc_01_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            var jsonRpcMessage = new JsonRpcResponse("foo", "000000000019d6689c085ae165831e934ff763ae46a2a6c172b3f1b60a8ce26f");
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        #endregion

        #region Example V1 T02: Bitcoin method "getblockhash" with error result

        [TestMethod]
        public void V1BitcoinT02DeserializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_btc_02_req.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jsonRpcContractResolver.AddRequestContract("getblockhash", new JsonRpcRequestContract(new[] { typeof(long) }));

            var jsonRpcData = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcMessageInfo = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual("foo", jsonRpcMessage.Id);
            Assert.AreEqual("getblockhash", jsonRpcMessage.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jsonRpcMessage.ParametersType);

            CollectionAssert.AreEqual(new object[] { -1L }, jsonRpcMessage.ParametersByPosition?.ToArray());
        }

        [TestMethod]
        public void V1BitcoinT02SerializeRequest()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_btc_02_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            var jsonRpcMessage = new JsonRpcRequest("foo", "getblockhash", new object[] { -1L });
            var jsonResult = jsonRpcSerializer.SerializeRequest(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V1BitcoinT02DeserializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_btc_02_res.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jsonRpcContractResolver.AddResponseContract("getblockhash", new JsonRpcResponseContract(typeof(string)));
            jsonRpcContractResolver.AddResponseBinding("foo", "getblockhash");

            var jsonRpcData = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcMessageInfo = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.AreEqual("foo", jsonRpcMessage.Id);
            Assert.IsFalse(jsonRpcMessage.Success);

            var jsonRpcError = jsonRpcMessage.Error;

            Assert.AreEqual(-8L, jsonRpcError.Code);
            Assert.AreEqual("Block height out of range", jsonRpcError.Message);
        }

        [TestMethod]
        public void V1BitcoinT02SerializeResponse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_btc_02_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            var jsonRpcMessage = new JsonRpcResponse("foo", new JsonRpcError(-8L, "Block height out of range"));
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        #endregion

        // 1.0 Core tests

        [TestMethod]
        public void V1CoreSerializeRequestWhenParametersAreByName()
        {
            var jsonRpcSerializer = new JsonRpcSerializer(compatibilityLevel: JsonRpcCompatibilityLevel.Level1);
            var parameters = new Dictionary<string, object> { ["p"] = 1L };
            var message = new JsonRpcRequest(default, "m", parameters);

            var exception = Assert.ThrowsException<JsonRpcSerializationException>(() =>
                jsonRpcSerializer.SerializeRequest(message));

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, exception.ErrorCode);
        }

        [TestMethod]
        public void V1CoreDeserializeResponseWhenErrorTypeIsInvali()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_core_error_type_invalid.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jsonRpcContractResolver.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jsonRpcContractResolver.AddResponseBinding(1L, "m");

            var jsonRpcData = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcMessageInfo = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcMessageInfo.IsValid);

            var jsonRpcMessage = jsonRpcMessageInfo.Message;

            Assert.IsFalse(jsonRpcMessage.Success);
            Assert.AreEqual(1L, jsonRpcMessage.Id);
            Assert.IsNull(jsonRpcMessage.Result);
            Assert.IsNotNull(jsonRpcMessage.Error);

            var jsonRpcError = jsonRpcMessage.Error;

            Assert.AreEqual(default, jsonRpcError.Code);
            Assert.AreEqual(string.Empty, jsonRpcError.Message);
            Assert.IsFalse(jsonRpcError.HasData);
        }

        [TestMethod]
        public void V1CoreDeserializeResponseWhenStructureIsInvalidAndSuccessIsFalse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_core_struct_invalid_success_false.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jsonRpcContractResolver.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jsonRpcContractResolver.AddResponseBinding(1L, "m");

            var jsonRpcData = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcMessageInfo = jsonRpcData.Item;

            Assert.IsFalse(jsonRpcMessageInfo.IsValid);

            var jsonRpcException = jsonRpcMessageInfo.Exception;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jsonRpcException.ErrorCode);
        }

        [TestMethod]
        public void V1CoreDeserializeResponseWhenStructureIsInvalidAndSuccessIsTrue()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v1_core_struct_invalid_success_true.json");
            var jsonRpcContractResolver = new JsonRpcContractResolver();
            var jsonRpcSerializer = new JsonRpcSerializer(jsonRpcContractResolver, compatibilityLevel: JsonRpcCompatibilityLevel.Level1);

            jsonRpcContractResolver.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jsonRpcContractResolver.AddResponseBinding(1L, "m");

            var jsonRpcData = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcMessageInfo = jsonRpcData.Item;

            Assert.IsFalse(jsonRpcMessageInfo.IsValid);

            var jsonRpcException = jsonRpcMessageInfo.Exception;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jsonRpcException.ErrorCode);
        }
    }
}