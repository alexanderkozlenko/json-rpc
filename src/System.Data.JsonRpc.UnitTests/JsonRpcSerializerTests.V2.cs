using System.Data.JsonRpc.UnitTests.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Data.JsonRpc.UnitTests
{
    public partial class JsonRpcSerializerTests
    {
        // 2.0 Core tests

        [TestMethod]
        public void V2CoreDeserializeRequestDataWhenParametersHasInvalidType()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_params_type_invalid.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            jsonRpcSerializer.RequestContracts["m"] = new JsonRpcRequestContract(new[] { typeof(object) });

            var jsonRpcData = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcItem = jsonRpcData.Item;

            Assert.IsFalse(jsonRpcItem.IsValid);

            var jsonRpcException = jsonRpcItem.Exception;

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, jsonRpcException.ErrorCode);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseDataWhenErrorTypeIsInvalid()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_type_invalid.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            jsonRpcSerializer.ResponseContracts["m"] = new JsonRpcResponseContract(typeof(string));
            jsonRpcSerializer.StaticResponseBindings[1L] = "m";

            var jsonRpcData = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcItem = jsonRpcData.Item;

            Assert.IsFalse(jsonRpcItem.IsValid);

            var jsonRpcException = jsonRpcItem.Exception;

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, jsonRpcException.ErrorCode);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseDataWhenErrorCodeIsReserved()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_code_reserved.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            jsonRpcSerializer.ResponseContracts["m"] = new JsonRpcResponseContract(typeof(string));
            jsonRpcSerializer.StaticResponseBindings[1L] = "m";

            var jsonRpcData = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcItem = jsonRpcData.Item;

            Assert.IsFalse(jsonRpcItem.IsValid);

            var jsonRpcException = jsonRpcItem.Exception;

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, jsonRpcException.ErrorCode);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseDataWhenErrorMessageIsNull()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_message_null.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            jsonRpcSerializer.ResponseContracts["m"] = new JsonRpcResponseContract(typeof(string));
            jsonRpcSerializer.StaticResponseBindings[1L] = "m";

            var jsonRpcData = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcItem = jsonRpcData.Item;

            Assert.IsFalse(jsonRpcItem.IsValid);

            var jsonRpcException = jsonRpcItem.Exception;

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, jsonRpcException.ErrorCode);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseDataWhenHasDataIsFalse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_has_data_false.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            jsonRpcSerializer.ResponseContracts["m"] = new JsonRpcResponseContract(typeof(string));
            jsonRpcSerializer.StaticResponseBindings[1L] = "m";

            var jsonRpcData = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcItem = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcItem.IsValid);

            var jsonRpcMessage = jsonRpcItem.Message;

            Assert.IsFalse(jsonRpcMessage.Success);

            var jsonRpcError = jsonRpcMessage.Error;

            Assert.IsFalse(jsonRpcError.HasData);
           Assert.IsNull(jsonRpcError.Data);
        }

        [TestMethod]
        public void V2CoreSerializeResponseDataWhenHasDataIsFalse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_has_data_false.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcError = new JsonRpcError(0L, "m");
            var jsonRpcMessage = new JsonRpcResponse(jsonRpcError, 1L);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseDataWhenHasDataIsTrue()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_has_data_true.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            jsonRpcSerializer.ResponseContracts["m"] = new JsonRpcResponseContract(typeof(string), typeof(long));
            jsonRpcSerializer.StaticResponseBindings[1L] = "m";

            var jsonRpcData = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcItem = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcItem.IsValid);

            var jsonRpcMessage = jsonRpcItem.Message;

            Assert.IsFalse(jsonRpcMessage.Success);

            var jsonRpcError = jsonRpcMessage.Error;

            Assert.IsTrue(jsonRpcError.HasData);
            Assert.AreEqual(0L, jsonRpcError.Data);
        }

        [TestMethod]
        public void V2CoreSerializeResponseDataWhenHasDataIsTrue()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_has_data_true.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcError = new JsonRpcError(0L, "m", 0L);
            var jsonRpcMessage = new JsonRpcResponse(jsonRpcError, 1L);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseDataWhenHasDataIsTrueAnsIsNull()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_has_data_true_null.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            jsonRpcSerializer.ResponseContracts["m"] = new JsonRpcResponseContract(typeof(string), typeof(string));
            jsonRpcSerializer.StaticResponseBindings[1L] = "m";

            var jsonRpcData = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcItem = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcItem.IsValid);

            var jsonRpcMessage = jsonRpcItem.Message;

            Assert.IsFalse(jsonRpcMessage.Success);

            var jsonRpcError = jsonRpcMessage.Error;

            Assert.IsTrue(jsonRpcError.HasData);
           Assert.IsNull(jsonRpcError.Data);
        }

        [TestMethod]
        public void V2CoreSerializeResponseDataWhenHasDataIsTrueAnsIsNull()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_error_has_data_true_null.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcError = new JsonRpcError(0L, "m", null);
            var jsonRpcMessage = new JsonRpcResponse(jsonRpcError, 1L);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2CoreDeserializeRequestDataWhenIdTypeIsInvalid()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_id_type_invalid_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            jsonRpcSerializer.RequestContracts["m"] = new JsonRpcRequestContract();

            var jsonRpcData = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcItem = jsonRpcData.Item;

            Assert.IsFalse(jsonRpcItem.IsValid);

            var jsonRpcException = jsonRpcItem.Exception;

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, jsonRpcException.ErrorCode);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseDataWhenIdTypeIsInvalid()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_id_type_invalid_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            jsonRpcSerializer.ResponseContracts["m"] = new JsonRpcResponseContract(typeof(string));
            jsonRpcSerializer.StaticResponseBindings[1D] = "m";

            var jsonRpcData = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcItem = jsonRpcData.Item;

            Assert.IsFalse(jsonRpcItem.IsValid);

            var jsonRpcException = jsonRpcItem.Exception;

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, jsonRpcException.ErrorCode);
        }

        [TestMethod]
        public void V2CoreSerializeRequestWhenIdIsFloat()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_id_float_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcRequest("m", 1D);
            var jsonResult = jsonRpcSerializer.SerializeRequest(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2CoreDeserializeRequestDataWhenIdIsFloat()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_id_float_req.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            jsonRpcSerializer.RequestContracts["m"] = new JsonRpcRequestContract();

            var jsonRpcData = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcItem = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcItem.IsValid);

            var jsonRpcMessage = jsonRpcItem.Message;
            var jsonRpcId = jsonRpcMessage.Id;

            Assert.AreEqual(JsonRpcIdType.Float, jsonRpcId.Type);
            Assert.AreEqual(1D, (double)jsonRpcId);
        }

        [TestMethod]
        public void V2CoreSerializeResponseWhenIdIsFloat()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_id_float_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcMessage = new JsonRpcResponse(string.Empty, 1D);
            var jsonResult = jsonRpcSerializer.SerializeResponse(jsonRpcMessage);

            CompareJsonStrings(jsonSample, jsonResult);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseDataWhenIdIsFloat()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_id_float_res.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            jsonRpcSerializer.ResponseContracts["m"] = new JsonRpcResponseContract(typeof(string));
            jsonRpcSerializer.StaticResponseBindings[1D] = "m";

            var jsonRpcData = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcItem = jsonRpcData.Item;

            Assert.IsTrue(jsonRpcItem.IsValid);

            var jsonRpcMessage = jsonRpcItem.Message;
            var jsonRpcId = jsonRpcMessage.Id;

            Assert.AreEqual(JsonRpcIdType.Float, jsonRpcId.Type);
            Assert.AreEqual(1D, (double)jsonRpcId);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseWhenStructureIsInvalidAndSuccessIsFalse()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_struct_invalid_success_false.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            jsonRpcSerializer.ResponseContracts["m"] = new JsonRpcResponseContract(typeof(string));
            jsonRpcSerializer.StaticResponseBindings[1L] = "m";

            var jsonRpcData = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcItem = jsonRpcData.Item;

            Assert.IsFalse(jsonRpcItem.IsValid);

            var jsonRpcException = jsonRpcItem.Exception;

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, jsonRpcException.ErrorCode);
        }

        [TestMethod]
        public void V2CoreDeserializeResponseWhenStructureIsInvalidAndSuccessIsTrue()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_core_struct_invalid_success_true.json");
            var jsonRpcSerializer = new JsonRpcSerializer();

            jsonRpcSerializer.ResponseContracts["m"] = new JsonRpcResponseContract(typeof(string));
            jsonRpcSerializer.StaticResponseBindings[1L] = "m";

            var jsonRpcData = jsonRpcSerializer.DeserializeResponseData(jsonSample);

            Assert.IsFalse(jsonRpcData.IsBatch);

            var jsonRpcItem = jsonRpcData.Item;

            Assert.IsFalse(jsonRpcItem.IsValid);

            var jsonRpcException = jsonRpcItem.Exception;

            Assert.AreEqual(JsonRpcErrorCodes.InvalidMessage, jsonRpcException.ErrorCode);
        }
    }
}