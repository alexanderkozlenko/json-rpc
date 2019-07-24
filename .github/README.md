# Anemonis.JsonRpc

Provides support for serializing and deserializing [JSON-RPC 2.0](http://www.jsonrpc.org/specification) messages.

[![NuGet](https://img.shields.io/nuget/vpre/Anemonis.JsonRpc.svg?style=flat-square)](https://www.nuget.org/packages/Anemonis.JsonRpc)
[![MyGet](https://img.shields.io/myget/alexanderkozlenko/vpre/Anemonis.JsonRpc.svg?label=myget&style=flat-square)](https://www.myget.org/feed/alexanderkozlenko/package/nuget/Anemonis.JsonRpc)
[![LGTM Alerts](https://img.shields.io/lgtm/alerts/github/alexanderkozlenko/json-rpc.svg?style=flat-square)](https://lgtm.com/projects/g/alexanderkozlenko/json-rpc)
[![Gitter](https://img.shields.io/gitter/room/nwjs/nw.js.svg?style=flat-square)](https://gitter.im/anemonis/json-rpc)

## Project Details

- Supports dynamic response type contracts based on method parameters.
- Provides limited backward compatibility with the [JSON-RPC 1.0](http://www.jsonrpc.org/specification_v1) protocol.

## Code Examples

```cs
var contracts = new JsonRpcContractResolver();
var serializer = new JsonRpcSerializer(contracts);

contracts.AddResponseContract("sum", new JsonRpcResponseContract(typeof(long)));

var request = new JsonRpcRequest(1L, "sum", new[] { 1L, 2L });
var requestString = serializer.SerializeRequest(request);

// [Executing the corresponding HTTP request]

contracts.AddResponseBinding(request.Id, request.Method);

var responseData = serializer.DeserializeResponseData(responseString);
var response = responseData.Item.Message;

Console.WriteLine((long)response.Result);
```

- Client-side usage: https://github.com/alexanderkozlenko/json-rpc-client
- Server-side usage: https://github.com/alexanderkozlenko/aspnetcore-json-rpc

## Quicklinks

- [Contributing Guidelines](./CONTRIBUTING.md)
- [Code of Conduct](./CODE_OF_CONDUCT.md)
