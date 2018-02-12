﻿using System.Data.JsonRpc.Tests.Resources;
using Xunit;

namespace System.Data.JsonRpc.Tests
{
    public sealed class JsonRpcDataTests
    {
        [Fact]
        public void GetItemAndItemsWhenIsSingle()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_data_single.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcData = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.False(jsonRpcData.IsBatch);
            Assert.Null(jsonRpcData.Items);
        }

        [Fact]
        public void GetItemAndItemsWhenIsBatch()
        {
            var jsonSample = EmbeddedResourceManager.GetString("Assets.v2_data_batch.json");
            var jsonRpcSerializer = new JsonRpcSerializer();
            var jsonRpcData = jsonRpcSerializer.DeserializeRequestData(jsonSample);

            Assert.True(jsonRpcData.IsBatch);
            Assert.NotNull(jsonRpcData.Items);
        }
    }
}