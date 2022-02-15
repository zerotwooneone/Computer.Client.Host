namespace MapsterTest.Domain;

public class ModelA
{
    public ModelB ModelB { get; set; }
    public ModelC ModelC { get; set; }
    public long? A { get; set; }
    public string DomainOnly { get; set; } = "this is initialized on the domain only";
}

public class ModelC
{
    public int C { get; set; }
}

public class ModelB
{
    public string B { get; set; } = "Not set";
}

