using System.Collections.Generic;
using Anemonis.JsonRpc.UnitTests.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anemonis.JsonRpc.UnitTests
{
    public partial class JsonRpcSerializerTests
    {
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