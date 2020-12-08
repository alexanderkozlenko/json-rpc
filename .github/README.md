# Anemonis.JsonRpc

Provides support for serializing and deserializing [JSON-RPC 2.0](http://www.jsonrpc.org/specification) messages.

| | Release | Current |
|---|---|---|
| Artifacts | [![](https://img.shields.io/nuget/vpre/Anemonis.JsonRpc.svg?style=flat-square)](https://www.nuget.org/packages/Anemonis.JsonRpc) | [![](https://img.shields.io/myget/alexanderkozlenko/vpre/Anemonis.JsonRpc.svg?label=myget&style=flat-square)](https://www.myget.org/feed/alexanderkozlenko/package/nuget/Anemonis.JsonRpc) |
| Code Health | | [![](https://img.shields.io/sonar/coverage/json-rpc?format=long&server=https%3A%2F%2Fsonarcloud.io&style=flat-square)](https://sonarcloud.io/component_measures?id=json-rpc&metric=coverage&view=list) [![](https://img.shields.io/sonar/violations/json-rpc?format=long&server=https%3A%2F%2Fsonarcloud.io&style=flat-square)](https://sonarcloud.io/project/issues?id=json-rpc&resolved=false) |
| Build Status | | [![](https://img.shields.io/azure-devops/build/alexanderkozlenko/github-pipelines/1?label=main&style=flat-square)](https://dev.azure.com/alexanderkozlenko/github-pipelines/_build?definitionId=1&_a=summary) |

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

// ...

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
