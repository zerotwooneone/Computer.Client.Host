// See https://aka.ms/new-console-template for more information


using Mapster;
using Newtonsoft.Json;

Console.WriteLine("Hello, World!");

var source = new MapsterTest.Proto.ProtoModelA
{
    A = 9,
    // ModelB = new MapsterTest.Proto.ProtoModelB
    // {
    //     B = "this was proto b"
    // },
    ModelC = new MapsterTest.Proto.ProtoModelC
    {
        C = 3
    },
    ProtoOnly = "This is proto only"
};

var domainTest = source.Adapt<MapsterTest.Domain.ModelA>();
Console.WriteLine(JsonConvert.SerializeObject(domainTest, Formatting.Indented));

var domainSource = new MapsterTest.Domain.ModelA
{
    A = 7,
    ModelB = new MapsterTest.Domain.ModelB
    {
        B = "this was domain b"
    },
    ModelC = new MapsterTest.Domain.ModelC
    {
        C = 77
    },
    DomainOnly = "This is Domain only"
};

Console.WriteLine();

var protoTest = domainSource.Adapt<MapsterTest.Proto.ProtoModelA>();
Console.WriteLine(JsonConvert.SerializeObject(protoTest, Formatting.Indented));