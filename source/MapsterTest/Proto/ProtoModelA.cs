using ProtoBuf;

namespace MapsterTest.Proto;

[ProtoContract]
public class ProtoModelA
{
    [ProtoMember(1)]
    public ProtoModelB? ModelB { get; set; }
    [ProtoMember(2)]
    public ProtoModelC? ModelC { get; set; }
    [ProtoMember(3)]
    public long? A { get; set; }
    [ProtoMember(4)]
    public string? ProtoOnly { get; set; } = "this is initialized on the proto only";
}

[ProtoContract]
public class ProtoModelC
{
    [ProtoMember(1)]
    public int? C { get; set; }
}

[ProtoContract]
public class ProtoModelB
{
    [ProtoMember(1)]
    public string? B { get; set; } = "Not set";
}