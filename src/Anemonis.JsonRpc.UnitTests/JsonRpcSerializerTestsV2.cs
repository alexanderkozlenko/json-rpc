using System;
using System.Collections.Generic;
using Anemonis.JsonRpc.UnitTests.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anemonis.JsonRpc.UnitTests
{
    [TestClass]
    public sealed class JsonRpcSerializerTestsV2
    {
        [TestMethod]
        public void DeserializeRequestDataTCB0I0P0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b0i0p0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract());

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(default, jrm.Id);
            Assert.AreEqual("m", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.None, jrm.ParametersType);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB0I0P1()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b0i0p1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new[] { typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(default, jrm.Id);
            Assert.AreEqual("m", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm.ParametersType);
            Assert.IsNotNull(jrm.ParametersByPosition);
            Assert.AreEqual(1, jrm.ParametersByPosition.Count);
            Assert.AreEqual(1L, jrm.ParametersByPosition[0]);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB0I0P2()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b0i0p2.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new Dictionary<string, Type> { ["p"] = typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(default, jrm.Id);
            Assert.AreEqual("m", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByName, jrm.ParametersType);
            Assert.IsNotNull(jrm.ParametersByName);
            Assert.AreEqual(1, jrm.ParametersByName.Count);
            Assert.AreEqual(1L, jrm.ParametersByName["p"]);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB0I1P0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b0i1p0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract());

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual("1", jrm.Id);
            Assert.AreEqual("m", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.None, jrm.ParametersType);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB0I1P1()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b0i1p1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new[] { typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual("1", jrm.Id);
            Assert.AreEqual("m", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm.ParametersType);
            Assert.IsNotNull(jrm.ParametersByPosition);
            Assert.AreEqual(1, jrm.ParametersByPosition.Count);
            Assert.AreEqual(1L, jrm.ParametersByPosition[0]);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB0I1P2()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b0i1p2.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new Dictionary<string, Type> { ["p"] = typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual("1", jrm.Id);
            Assert.AreEqual("m", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByName, jrm.ParametersType);
            Assert.IsNotNull(jrm.ParametersByName);
            Assert.AreEqual(1, jrm.ParametersByName.Count);
            Assert.AreEqual(1L, jrm.ParametersByName["p"]);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB0I2P0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b0i2p0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract());

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(1L, jrm.Id);
            Assert.AreEqual("m", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.None, jrm.ParametersType);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB0I2P1()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b0i2p1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new[] { typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(1L, jrm.Id);
            Assert.AreEqual("m", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm.ParametersType);
            Assert.IsNotNull(jrm.ParametersByPosition);
            Assert.AreEqual(1, jrm.ParametersByPosition.Count);
            Assert.AreEqual(1L, jrm.ParametersByPosition[0]);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB0I2P2()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b0i2p2.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new Dictionary<string, Type> { ["p"] = typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(1L, jrm.Id);
            Assert.AreEqual("m", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByName, jrm.ParametersType);
            Assert.IsNotNull(jrm.ParametersByName);
            Assert.AreEqual(1, jrm.ParametersByName.Count);
            Assert.AreEqual(1L, jrm.ParametersByName["p"]);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB0I3P0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b0i3p0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract());

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(1D, jrm.Id);
            Assert.AreEqual("m", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.None, jrm.ParametersType);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB0I3P1()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b0i3p1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new[] { typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(1D, jrm.Id);
            Assert.AreEqual("m", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm.ParametersType);
            Assert.IsNotNull(jrm.ParametersByPosition);
            Assert.AreEqual(1, jrm.ParametersByPosition.Count);
            Assert.AreEqual(1L, jrm.ParametersByPosition[0]);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB0I3P2()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b0i3p2.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new Dictionary<string, Type> { ["p"] = typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(1D, jrm.Id);
            Assert.AreEqual("m", jrm.Method);
            Assert.AreEqual(JsonRpcParametersType.ByName, jrm.ParametersType);
            Assert.IsNotNull(jrm.ParametersByName);
            Assert.AreEqual(1, jrm.ParametersByName.Count);
            Assert.AreEqual(1L, jrm.ParametersByName["p"]);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB1I0P0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b1i0p0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract());

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(default, jrm0.Id);
            Assert.AreEqual("m", jrm0.Method);
            Assert.AreEqual(JsonRpcParametersType.None, jrm0.ParametersType);

            Assert.AreEqual(default, jrm1.Id);
            Assert.AreEqual("m", jrm1.Method);
            Assert.AreEqual(JsonRpcParametersType.None, jrm1.ParametersType);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB1I0P1()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b1i0p1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new[] { typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(default, jrm0.Id);
            Assert.AreEqual("m", jrm0.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm0.ParametersType);
            Assert.IsNotNull(jrm0.ParametersByPosition);
            Assert.AreEqual(1, jrm0.ParametersByPosition.Count);
            Assert.AreEqual(1L, jrm0.ParametersByPosition[0]);

            Assert.AreEqual(default, jrm1.Id);
            Assert.AreEqual("m", jrm1.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm1.ParametersType);
            Assert.IsNotNull(jrm1.ParametersByPosition);
            Assert.AreEqual(1, jrm1.ParametersByPosition.Count);
            Assert.AreEqual(1L, jrm1.ParametersByPosition[0]);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB1I0P2()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b1i0p2.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new Dictionary<string, Type> { ["p"] = typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(default, jrm0.Id);
            Assert.AreEqual("m", jrm0.Method);
            Assert.AreEqual(JsonRpcParametersType.ByName, jrm0.ParametersType);
            Assert.IsNotNull(jrm0.ParametersByName);
            Assert.AreEqual(1, jrm0.ParametersByName.Count);
            Assert.AreEqual(1L, jrm0.ParametersByName["p"]);

            Assert.AreEqual(default, jrm1.Id);
            Assert.AreEqual("m", jrm1.Method);
            Assert.AreEqual(JsonRpcParametersType.ByName, jrm1.ParametersType);
            Assert.IsNotNull(jrm1.ParametersByName);
            Assert.AreEqual(1, jrm1.ParametersByName.Count);
            Assert.AreEqual(1L, jrm1.ParametersByName["p"]);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB1I1P0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b1i1p0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract());

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual("1", jrm0.Id);
            Assert.AreEqual("m", jrm0.Method);
            Assert.AreEqual(JsonRpcParametersType.None, jrm0.ParametersType);

            Assert.AreEqual("2", jrm1.Id);
            Assert.AreEqual("m", jrm1.Method);
            Assert.AreEqual(JsonRpcParametersType.None, jrm1.ParametersType);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB1I1P1()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b1i1p1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new[] { typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual("1", jrm0.Id);
            Assert.AreEqual("m", jrm0.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm0.ParametersType);
            Assert.IsNotNull(jrm0.ParametersByPosition);
            Assert.AreEqual(1, jrm0.ParametersByPosition.Count);
            Assert.AreEqual(1L, jrm0.ParametersByPosition[0]);

            Assert.AreEqual("2", jrm1.Id);
            Assert.AreEqual("m", jrm1.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm1.ParametersType);
            Assert.IsNotNull(jrm1.ParametersByPosition);
            Assert.AreEqual(1, jrm1.ParametersByPosition.Count);
            Assert.AreEqual(1L, jrm1.ParametersByPosition[0]);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB1I1P2()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b1i1p2.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new Dictionary<string, Type> { ["p"] = typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual("1", jrm0.Id);
            Assert.AreEqual("m", jrm0.Method);
            Assert.AreEqual(JsonRpcParametersType.ByName, jrm0.ParametersType);
            Assert.IsNotNull(jrm0.ParametersByName);
            Assert.AreEqual(1, jrm0.ParametersByName.Count);
            Assert.AreEqual(1L, jrm0.ParametersByName["p"]);

            Assert.AreEqual("2", jrm1.Id);
            Assert.AreEqual("m", jrm1.Method);
            Assert.AreEqual(JsonRpcParametersType.ByName, jrm1.ParametersType);
            Assert.IsNotNull(jrm1.ParametersByName);
            Assert.AreEqual(1, jrm1.ParametersByName.Count);
            Assert.AreEqual(1L, jrm1.ParametersByName["p"]);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB1I2P0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b1i2p0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract());

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(1L, jrm0.Id);
            Assert.AreEqual("m", jrm0.Method);
            Assert.AreEqual(JsonRpcParametersType.None, jrm0.ParametersType);

            Assert.AreEqual(2L, jrm1.Id);
            Assert.AreEqual("m", jrm1.Method);
            Assert.AreEqual(JsonRpcParametersType.None, jrm1.ParametersType);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB1I2P1()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b1i2p1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new[] { typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(1L, jrm0.Id);
            Assert.AreEqual("m", jrm0.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm0.ParametersType);
            Assert.IsNotNull(jrm0.ParametersByPosition);
            Assert.AreEqual(1, jrm0.ParametersByPosition.Count);
            Assert.AreEqual(1L, jrm0.ParametersByPosition[0]);

            Assert.AreEqual(2L, jrm1.Id);
            Assert.AreEqual("m", jrm1.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm1.ParametersType);
            Assert.IsNotNull(jrm1.ParametersByPosition);
            Assert.AreEqual(1, jrm1.ParametersByPosition.Count);
            Assert.AreEqual(1L, jrm1.ParametersByPosition[0]);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB1I2P2()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b1i2p2.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new Dictionary<string, Type> { ["p"] = typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(1L, jrm0.Id);
            Assert.AreEqual("m", jrm0.Method);
            Assert.AreEqual(JsonRpcParametersType.ByName, jrm0.ParametersType);
            Assert.IsNotNull(jrm0.ParametersByName);
            Assert.AreEqual(1, jrm0.ParametersByName.Count);
            Assert.AreEqual(1L, jrm0.ParametersByName["p"]);

            Assert.AreEqual(2L, jrm1.Id);
            Assert.AreEqual("m", jrm1.Method);
            Assert.AreEqual(JsonRpcParametersType.ByName, jrm1.ParametersType);
            Assert.IsNotNull(jrm1.ParametersByName);
            Assert.AreEqual(1, jrm1.ParametersByName.Count);
            Assert.AreEqual(1L, jrm1.ParametersByName["p"]);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB1I3P0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b1i3p0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract());

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(1D, jrm0.Id);
            Assert.AreEqual("m", jrm0.Method);
            Assert.AreEqual(JsonRpcParametersType.None, jrm0.ParametersType);

            Assert.AreEqual(2D, jrm1.Id);
            Assert.AreEqual("m", jrm1.Method);
            Assert.AreEqual(JsonRpcParametersType.None, jrm1.ParametersType);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB1I3P1()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b1i3p1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new[] { typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(1D, jrm0.Id);
            Assert.AreEqual("m", jrm0.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm0.ParametersType);
            Assert.IsNotNull(jrm0.ParametersByPosition);
            Assert.AreEqual(1, jrm0.ParametersByPosition.Count);
            Assert.AreEqual(1L, jrm0.ParametersByPosition[0]);

            Assert.AreEqual(2D, jrm1.Id);
            Assert.AreEqual("m", jrm1.Method);
            Assert.AreEqual(JsonRpcParametersType.ByPosition, jrm1.ParametersType);
            Assert.IsNotNull(jrm1.ParametersByPosition);
            Assert.AreEqual(1, jrm1.ParametersByPosition.Count);
            Assert.AreEqual(1L, jrm1.ParametersByPosition[0]);
        }

        [TestMethod]
        public void DeserializeRequestDataTCB1I3P2()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_req_b1i3p2.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new Dictionary<string, Type> { ["p"] = typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(1D, jrm0.Id);
            Assert.AreEqual("m", jrm0.Method);
            Assert.AreEqual(JsonRpcParametersType.ByName, jrm0.ParametersType);
            Assert.IsNotNull(jrm0.ParametersByName);
            Assert.AreEqual(1, jrm0.ParametersByName.Count);
            Assert.AreEqual(1L, jrm0.ParametersByName["p"]);

            Assert.AreEqual(2D, jrm1.Id);
            Assert.AreEqual("m", jrm1.Method);
            Assert.AreEqual(JsonRpcParametersType.ByName, jrm1.ParametersType);
            Assert.IsNotNull(jrm1.ParametersByName);
            Assert.AreEqual(1, jrm1.ParametersByName.Count);
            Assert.AreEqual(1L, jrm1.ParametersByName["p"]);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB0I0E0D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b0i0e0d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract("m", new JsonRpcResponseContract(typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsFalse(jrd.Item.IsValid);
            Assert.IsNull(jrd.Item.Message);
            Assert.IsNotNull(jrd.Item.Exception);

            var jre = jrd.Item.Exception;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre.ErrorCode);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB0I0E1D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b0i0e1d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(default(JsonRpcId), new JsonRpcResponseContract(typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(default, jrm.Id);
            Assert.IsFalse(jrm.Success);
            Assert.IsNotNull(jrm.Error);
            Assert.AreEqual(1L, jrm.Error.Code);
            Assert.AreEqual("m", jrm.Error.Message);
            Assert.IsFalse(jrm.Error.HasData);
            Assert.IsNull(jrm.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB0I0E1D1()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b0i0e1d1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(default(JsonRpcId), new JsonRpcResponseContract(typeof(long), typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(default, jrm.Id);
            Assert.IsFalse(jrm.Success);
            Assert.IsNotNull(jrm.Error);
            Assert.AreEqual(1L, jrm.Error.Code);
            Assert.AreEqual("m", jrm.Error.Message);
            Assert.IsTrue(jrm.Error.HasData);
            Assert.AreEqual(0L, jrm.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB0I0E1D2()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b0i0e1d2.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(default(JsonRpcId), new JsonRpcResponseContract(typeof(long), typeof(string)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(default, jrm.Id);
            Assert.IsFalse(jrm.Success);
            Assert.IsNotNull(jrm.Error);
            Assert.AreEqual(1L, jrm.Error.Code);
            Assert.AreEqual("m", jrm.Error.Message);
            Assert.IsTrue(jrm.Error.HasData);
            Assert.IsNull(jrm.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB0I1E0D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b0i1e0d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract((JsonRpcId)"1", new JsonRpcResponseContract(typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual("1", jrm.Id);
            Assert.IsTrue(jrm.Success);
            Assert.IsNull(jrm.Error);
            Assert.AreEqual(0L, jrm.Result);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB0I1E1D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b0i1e1d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract((JsonRpcId)"1", new JsonRpcResponseContract(typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual("1", jrm.Id);
            Assert.IsFalse(jrm.Success);
            Assert.IsNotNull(jrm.Error);
            Assert.AreEqual(1L, jrm.Error.Code);
            Assert.AreEqual("m", jrm.Error.Message);
            Assert.IsFalse(jrm.Error.HasData);
            Assert.IsNull(jrm.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB0I1E1D1()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b0i1e1d1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract((JsonRpcId)"1", new JsonRpcResponseContract(typeof(long), typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual("1", jrm.Id);
            Assert.IsFalse(jrm.Success);
            Assert.IsNotNull(jrm.Error);
            Assert.AreEqual(1L, jrm.Error.Code);
            Assert.AreEqual("m", jrm.Error.Message);
            Assert.IsTrue(jrm.Error.HasData);
            Assert.AreEqual(0L, jrm.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB0I1E1D2()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b0i1e1d2.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract((JsonRpcId)"1", new JsonRpcResponseContract(typeof(long), typeof(string)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual("1", jrm.Id);
            Assert.IsFalse(jrm.Success);
            Assert.IsNotNull(jrm.Error);
            Assert.AreEqual(1L, jrm.Error.Code);
            Assert.AreEqual("m", jrm.Error.Message);
            Assert.IsTrue(jrm.Error.HasData);
            Assert.IsNull(jrm.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB0I2E0D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b0i2e0d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(1L, new JsonRpcResponseContract(typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(1L, jrm.Id);
            Assert.IsTrue(jrm.Success);
            Assert.IsNull(jrm.Error);
            Assert.AreEqual(0L, jrm.Result);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB0I2E1D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b0i2e1d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(1L, new JsonRpcResponseContract(typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(1L, jrm.Id);
            Assert.IsFalse(jrm.Success);
            Assert.IsNotNull(jrm.Error);
            Assert.AreEqual(1L, jrm.Error.Code);
            Assert.AreEqual("m", jrm.Error.Message);
            Assert.IsFalse(jrm.Error.HasData);
            Assert.IsNull(jrm.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB0I2E1D1()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b0i2e1d1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(1L, new JsonRpcResponseContract(typeof(long), typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(1L, jrm.Id);
            Assert.IsFalse(jrm.Success);
            Assert.IsNotNull(jrm.Error);
            Assert.AreEqual(1L, jrm.Error.Code);
            Assert.AreEqual("m", jrm.Error.Message);
            Assert.IsTrue(jrm.Error.HasData);
            Assert.AreEqual(0L, jrm.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB0I2E1D2()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b0i2e1d2.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(1L, new JsonRpcResponseContract(typeof(long), typeof(string)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(1L, jrm.Id);
            Assert.IsFalse(jrm.Success);
            Assert.IsNotNull(jrm.Error);
            Assert.AreEqual(1L, jrm.Error.Code);
            Assert.AreEqual("m", jrm.Error.Message);
            Assert.IsTrue(jrm.Error.HasData);
            Assert.IsNull(jrm.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB0I3E0D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b0i3e0d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(1D, new JsonRpcResponseContract(typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(1D, jrm.Id);
            Assert.IsTrue(jrm.Success);
            Assert.IsNull(jrm.Error);
            Assert.AreEqual(0L, jrm.Result);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB0I3E1D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b0i3e1d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(1D, new JsonRpcResponseContract(typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(1D, jrm.Id);
            Assert.IsFalse(jrm.Success);
            Assert.IsNotNull(jrm.Error);
            Assert.AreEqual(1L, jrm.Error.Code);
            Assert.AreEqual("m", jrm.Error.Message);
            Assert.IsFalse(jrm.Error.HasData);
            Assert.IsNull(jrm.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB0I3E1D1()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b0i3e1d1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(1D, new JsonRpcResponseContract(typeof(long), typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(1D, jrm.Id);
            Assert.IsFalse(jrm.Success);
            Assert.IsNotNull(jrm.Error);
            Assert.AreEqual(1L, jrm.Error.Code);
            Assert.AreEqual("m", jrm.Error.Message);
            Assert.IsTrue(jrm.Error.HasData);
            Assert.AreEqual(0L, jrm.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB0I3E1D2()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b0i3e1d2.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(1D, new JsonRpcResponseContract(typeof(long), typeof(string)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsTrue(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Message);
            Assert.IsNull(jrd.Item.Exception);

            var jrm = jrd.Item.Message;

            Assert.AreEqual(1D, jrm.Id);
            Assert.IsFalse(jrm.Success);
            Assert.IsNotNull(jrm.Error);
            Assert.AreEqual(1L, jrm.Error.Code);
            Assert.AreEqual("m", jrm.Error.Message);
            Assert.IsTrue(jrm.Error.HasData);
            Assert.IsNull(jrm.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB1I0E0D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b1i0e0d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract("m", new JsonRpcResponseContract(typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsFalse(jrd.Items[0].IsValid);
            Assert.IsFalse(jrd.Items[1].IsValid);
            Assert.IsNull(jrd.Items[0].Message);
            Assert.IsNull(jrd.Items[1].Message);
            Assert.IsNotNull(jrd.Items[0].Exception);
            Assert.IsNotNull(jrd.Items[1].Exception);

            var jre0 = jrd.Items[0].Exception;
            var jre1 = jrd.Items[1].Exception;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre0.ErrorCode);

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre1.ErrorCode);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB1I0E1D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b1i0e1d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(default(JsonRpcId), new JsonRpcResponseContract(typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(default, jrm0.Id);
            Assert.IsFalse(jrm0.Success);
            Assert.IsNotNull(jrm0.Error);
            Assert.AreEqual(1L, jrm0.Error.Code);
            Assert.AreEqual("m", jrm0.Error.Message);
            Assert.IsFalse(jrm0.Error.HasData);
            Assert.IsNull(jrm0.Error.Data);

            Assert.AreEqual(default, jrm1.Id);
            Assert.IsFalse(jrm1.Success);
            Assert.IsNotNull(jrm1.Error);
            Assert.AreEqual(1L, jrm1.Error.Code);
            Assert.AreEqual("m", jrm1.Error.Message);
            Assert.IsFalse(jrm1.Error.HasData);
            Assert.IsNull(jrm1.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB1I0E1D1()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b1i0e1d1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(default(JsonRpcId), new JsonRpcResponseContract(typeof(long), typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(default, jrm0.Id);
            Assert.IsFalse(jrm0.Success);
            Assert.IsNotNull(jrm0.Error);
            Assert.AreEqual(1L, jrm0.Error.Code);
            Assert.AreEqual("m", jrm0.Error.Message);
            Assert.IsTrue(jrm0.Error.HasData);
            Assert.AreEqual(0L, jrm0.Error.Data);

            Assert.AreEqual(default, jrm1.Id);
            Assert.IsFalse(jrm1.Success);
            Assert.IsNotNull(jrm1.Error);
            Assert.AreEqual(1L, jrm1.Error.Code);
            Assert.AreEqual("m", jrm1.Error.Message);
            Assert.IsTrue(jrm1.Error.HasData);
            Assert.AreEqual(0L, jrm1.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB1I0E1D2()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b1i0e1d2.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(default(JsonRpcId), new JsonRpcResponseContract(typeof(long), typeof(string)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(default, jrm0.Id);
            Assert.IsFalse(jrm0.Success);
            Assert.IsNotNull(jrm0.Error);
            Assert.AreEqual(1L, jrm0.Error.Code);
            Assert.AreEqual("m", jrm0.Error.Message);
            Assert.IsTrue(jrm0.Error.HasData);
            Assert.IsNull(jrm0.Error.Data);

            Assert.AreEqual(default, jrm1.Id);
            Assert.IsFalse(jrm1.Success);
            Assert.IsNotNull(jrm1.Error);
            Assert.AreEqual(1L, jrm1.Error.Code);
            Assert.AreEqual("m", jrm1.Error.Message);
            Assert.IsTrue(jrm1.Error.HasData);
            Assert.IsNull(jrm1.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB1I1E0D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b1i1e0d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract((JsonRpcId)"1", new JsonRpcResponseContract(typeof(long)));
            jrcr.AddResponseContract((JsonRpcId)"2", new JsonRpcResponseContract(typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual("1", jrm0.Id);
            Assert.IsTrue(jrm0.Success);
            Assert.IsNull(jrm0.Error);
            Assert.AreEqual(0L, jrm0.Result);

            Assert.AreEqual("2", jrm1.Id);
            Assert.IsTrue(jrm1.Success);
            Assert.IsNull(jrm1.Error);
            Assert.AreEqual(0L, jrm1.Result);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB1I1E1D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b1i1e1d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract((JsonRpcId)"1", new JsonRpcResponseContract(typeof(long)));
            jrcr.AddResponseContract((JsonRpcId)"2", new JsonRpcResponseContract(typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual("1", jrm0.Id);
            Assert.IsFalse(jrm0.Success);
            Assert.IsNotNull(jrm0.Error);
            Assert.AreEqual(1L, jrm0.Error.Code);
            Assert.AreEqual("m", jrm0.Error.Message);
            Assert.IsFalse(jrm0.Error.HasData);
            Assert.IsNull(jrm0.Error.Data);

            Assert.AreEqual("2", jrm1.Id);
            Assert.IsFalse(jrm1.Success);
            Assert.IsNotNull(jrm1.Error);
            Assert.AreEqual(1L, jrm1.Error.Code);
            Assert.AreEqual("m", jrm1.Error.Message);
            Assert.IsFalse(jrm1.Error.HasData);
            Assert.IsNull(jrm1.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB1I1E1D1()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b1i1e1d1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract((JsonRpcId)"1", new JsonRpcResponseContract(typeof(long), typeof(long)));
            jrcr.AddResponseContract((JsonRpcId)"2", new JsonRpcResponseContract(typeof(long), typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual("1", jrm0.Id);
            Assert.IsFalse(jrm0.Success);
            Assert.IsNotNull(jrm0.Error);
            Assert.AreEqual(1L, jrm0.Error.Code);
            Assert.AreEqual("m", jrm0.Error.Message);
            Assert.IsTrue(jrm0.Error.HasData);
            Assert.AreEqual(0L, jrm0.Error.Data);

            Assert.AreEqual("2", jrm1.Id);
            Assert.IsFalse(jrm1.Success);
            Assert.IsNotNull(jrm1.Error);
            Assert.AreEqual(1L, jrm1.Error.Code);
            Assert.AreEqual("m", jrm1.Error.Message);
            Assert.IsTrue(jrm1.Error.HasData);
            Assert.AreEqual(0L, jrm1.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB1I1E1D2()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b1i1e1d2.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract((JsonRpcId)"1", new JsonRpcResponseContract(typeof(long), typeof(string)));
            jrcr.AddResponseContract((JsonRpcId)"2", new JsonRpcResponseContract(typeof(long), typeof(string)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual("1", jrm0.Id);
            Assert.IsFalse(jrm0.Success);
            Assert.IsNotNull(jrm0.Error);
            Assert.AreEqual(1L, jrm0.Error.Code);
            Assert.AreEqual("m", jrm0.Error.Message);
            Assert.IsTrue(jrm0.Error.HasData);
            Assert.IsNull(jrm0.Error.Data);

            Assert.AreEqual("2", jrm1.Id);
            Assert.IsFalse(jrm1.Success);
            Assert.IsNotNull(jrm1.Error);
            Assert.AreEqual(1L, jrm1.Error.Code);
            Assert.AreEqual("m", jrm1.Error.Message);
            Assert.IsTrue(jrm1.Error.HasData);
            Assert.IsNull(jrm1.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB1I2E0D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b1i2e0d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(1L, new JsonRpcResponseContract(typeof(long)));
            jrcr.AddResponseContract(2L, new JsonRpcResponseContract(typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(1L, jrm0.Id);
            Assert.IsTrue(jrm0.Success);
            Assert.IsNull(jrm0.Error);
            Assert.AreEqual(0L, jrm0.Result);

            Assert.AreEqual(2L, jrm1.Id);
            Assert.IsTrue(jrm1.Success);
            Assert.IsNull(jrm1.Error);
            Assert.AreEqual(0L, jrm1.Result);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB1I2E1D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b1i2e1d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(1L, new JsonRpcResponseContract(typeof(long)));
            jrcr.AddResponseContract(2L, new JsonRpcResponseContract(typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(1L, jrm0.Id);
            Assert.IsFalse(jrm0.Success);
            Assert.IsNotNull(jrm0.Error);
            Assert.AreEqual(1L, jrm0.Error.Code);
            Assert.AreEqual("m", jrm0.Error.Message);
            Assert.IsFalse(jrm0.Error.HasData);
            Assert.IsNull(jrm0.Error.Data);

            Assert.AreEqual(2L, jrm1.Id);
            Assert.IsFalse(jrm1.Success);
            Assert.IsNotNull(jrm1.Error);
            Assert.AreEqual(1L, jrm1.Error.Code);
            Assert.AreEqual("m", jrm1.Error.Message);
            Assert.IsFalse(jrm1.Error.HasData);
            Assert.IsNull(jrm1.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB1I2E1D1()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b1i2e1d1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(1L, new JsonRpcResponseContract(typeof(long), typeof(long)));
            jrcr.AddResponseContract(2L, new JsonRpcResponseContract(typeof(long), typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(1L, jrm0.Id);
            Assert.IsFalse(jrm0.Success);
            Assert.IsNotNull(jrm0.Error);
            Assert.AreEqual(1L, jrm0.Error.Code);
            Assert.AreEqual("m", jrm0.Error.Message);
            Assert.IsTrue(jrm0.Error.HasData);
            Assert.AreEqual(0L, jrm0.Error.Data);

            Assert.AreEqual(2L, jrm1.Id);
            Assert.IsFalse(jrm1.Success);
            Assert.IsNotNull(jrm1.Error);
            Assert.AreEqual(1L, jrm1.Error.Code);
            Assert.AreEqual("m", jrm1.Error.Message);
            Assert.IsTrue(jrm1.Error.HasData);
            Assert.AreEqual(0L, jrm1.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB1I2E1D2()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b1i2e1d2.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(1L, new JsonRpcResponseContract(typeof(long), typeof(string)));
            jrcr.AddResponseContract(2L, new JsonRpcResponseContract(typeof(long), typeof(string)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(1L, jrm0.Id);
            Assert.IsFalse(jrm0.Success);
            Assert.IsNotNull(jrm0.Error);
            Assert.AreEqual(1L, jrm0.Error.Code);
            Assert.AreEqual("m", jrm0.Error.Message);
            Assert.IsTrue(jrm0.Error.HasData);
            Assert.IsNull(jrm0.Error.Data);

            Assert.AreEqual(2L, jrm1.Id);
            Assert.IsFalse(jrm1.Success);
            Assert.IsNotNull(jrm1.Error);
            Assert.AreEqual(1L, jrm1.Error.Code);
            Assert.AreEqual("m", jrm1.Error.Message);
            Assert.IsTrue(jrm1.Error.HasData);
            Assert.IsNull(jrm1.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB1I3E0D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b1i3e0d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(1D, new JsonRpcResponseContract(typeof(long)));
            jrcr.AddResponseContract(2D, new JsonRpcResponseContract(typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(1D, jrm0.Id);
            Assert.IsTrue(jrm0.Success);
            Assert.IsNull(jrm0.Error);
            Assert.AreEqual(0L, jrm0.Result);

            Assert.AreEqual(2D, jrm1.Id);
            Assert.IsTrue(jrm1.Success);
            Assert.IsNull(jrm1.Error);
            Assert.AreEqual(0L, jrm1.Result);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB1I3E1D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b1i3e1d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(1D, new JsonRpcResponseContract(typeof(long)));
            jrcr.AddResponseContract(2D, new JsonRpcResponseContract(typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(1D, jrm0.Id);
            Assert.IsFalse(jrm0.Success);
            Assert.IsNotNull(jrm0.Error);
            Assert.AreEqual(1L, jrm0.Error.Code);
            Assert.AreEqual("m", jrm0.Error.Message);
            Assert.IsFalse(jrm0.Error.HasData);
            Assert.IsNull(jrm0.Error.Data);

            Assert.AreEqual(2D, jrm1.Id);
            Assert.IsFalse(jrm1.Success);
            Assert.IsNotNull(jrm1.Error);
            Assert.AreEqual(1L, jrm1.Error.Code);
            Assert.AreEqual("m", jrm1.Error.Message);
            Assert.IsFalse(jrm1.Error.HasData);
            Assert.IsNull(jrm1.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB1I3E1D1()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b1i3e1d1.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(1D, new JsonRpcResponseContract(typeof(long), typeof(long)));
            jrcr.AddResponseContract(2D, new JsonRpcResponseContract(typeof(long), typeof(long)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(1D, jrm0.Id);
            Assert.IsFalse(jrm0.Success);
            Assert.IsNotNull(jrm0.Error);
            Assert.AreEqual(1L, jrm0.Error.Code);
            Assert.AreEqual("m", jrm0.Error.Message);
            Assert.IsTrue(jrm0.Error.HasData);
            Assert.AreEqual(0L, jrm0.Error.Data);

            Assert.AreEqual(2D, jrm1.Id);
            Assert.IsFalse(jrm1.Success);
            Assert.IsNotNull(jrm1.Error);
            Assert.AreEqual(1L, jrm1.Error.Code);
            Assert.AreEqual("m", jrm1.Error.Message);
            Assert.IsTrue(jrm1.Error.HasData);
            Assert.AreEqual(0L, jrm1.Error.Data);
        }

        [TestMethod]
        public void DeserializeResponseDataTCB1I3E1D2()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_res_b1i3e1d2.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract(1D, new JsonRpcResponseContract(typeof(long), typeof(string)));
            jrcr.AddResponseContract(2D, new JsonRpcResponseContract(typeof(long), typeof(string)));

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsTrue(jrd.IsBatch);
            Assert.IsNotNull(jrd.Items);
            Assert.AreEqual(2, jrd.Items.Count);
            Assert.IsTrue(jrd.Items[0].IsValid);
            Assert.IsTrue(jrd.Items[1].IsValid);
            Assert.IsNotNull(jrd.Items[0].Message);
            Assert.IsNotNull(jrd.Items[1].Message);
            Assert.IsNull(jrd.Items[0].Exception);
            Assert.IsNull(jrd.Items[1].Exception);

            var jrm0 = jrd.Items[0].Message;
            var jrm1 = jrd.Items[1].Message;

            Assert.AreEqual(1D, jrm0.Id);
            Assert.IsFalse(jrm0.Success);
            Assert.IsNotNull(jrm0.Error);
            Assert.AreEqual(1L, jrm0.Error.Code);
            Assert.AreEqual("m", jrm0.Error.Message);
            Assert.IsTrue(jrm0.Error.HasData);
            Assert.IsNull(jrm0.Error.Data);

            Assert.AreEqual(2D, jrm1.Id);
            Assert.IsFalse(jrm1.Success);
            Assert.IsNotNull(jrm1.Error);
            Assert.AreEqual(1L, jrm1.Error.Code);
            Assert.AreEqual("m", jrm1.Error.Message);
            Assert.IsTrue(jrm1.Error.HasData);
            Assert.IsNull(jrm1.Error.Data);
        }

        [TestMethod]
        public void DeserializeRequestDataTIIIB0I0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_ii_req_b0i0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract());

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsFalse(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Exception);

            var jre = jrd.Item.Exception;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre.ErrorCode);
        }

        [TestMethod]
        public void DeserializeResponseDataTIIIB0I0E0D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_ii_res_b0i0e0d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jrcr.AddResponseBinding(1D, "m");

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsFalse(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Exception);

            var jre = jrd.Item.Exception;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre.ErrorCode);
        }

        [TestMethod]
        public void DeserializeRequestDataTIIPB0I0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_ip_req_b0i0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddRequestContract("m", new JsonRpcRequestContract(new[] { typeof(long) }));

            var jrd = jrs.DeserializeRequestData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsFalse(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Exception);

            var jre = jrd.Item.Exception;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre.ErrorCode);
        }

        [TestMethod]
        public void DeserializeResponseDataTIITB0I0E1D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_it_res_b0i0e1d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jrcr.AddResponseBinding(1L, "m");

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsFalse(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Exception);

            var jre = jrd.Item.Exception;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre.ErrorCode);
        }

        [TestMethod]
        public void DeserializeResponseDataTINMB0I0E1D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_nm_res_b0i0e1d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jrcr.AddResponseBinding(1L, "m");

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsFalse(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Exception);

            var jre = jrd.Item.Exception;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre.ErrorCode);
        }

        [TestMethod]
        public void DeserializeResponseDataTIRCB0I0E1D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_rc_res_b0i0e1d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jrcr.AddResponseBinding(1L, "m");

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsFalse(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Exception);

            var jre = jrd.Item.Exception;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre.ErrorCode);
        }

        [TestMethod]
        public void DeserializeResponseDataTIISB0I0E1D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_is_res_b0i0e1d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jrcr.AddResponseBinding(1D, "m");

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsFalse(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Exception);

            var jre = jrd.Item.Exception;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre.ErrorCode);
        }

        [TestMethod]
        public void DeserializeResponseDataTIISB0I0E0D0()
        {
            var jsont = EmbeddedResourceManager.GetString("Assets.v2_tc_is_res_b0i0e0d0.json");
            var jrcr = new JsonRpcContractResolver();
            var jrs = new JsonRpcSerializer(jrcr);

            jrcr.AddResponseContract("m", new JsonRpcResponseContract(typeof(string)));
            jrcr.AddResponseBinding(1D, "m");

            var jrd = jrs.DeserializeResponseData(jsont);

            Assert.IsNotNull(jrd);
            Assert.IsFalse(jrd.IsBatch);
            Assert.IsNull(jrd.Items);
            Assert.IsFalse(jrd.Item.IsValid);
            Assert.IsNotNull(jrd.Item.Exception);

            var jre = jrd.Item.Exception;

            Assert.AreEqual(JsonRpcErrorCode.InvalidMessage, jre.ErrorCode);
        }
    }
}