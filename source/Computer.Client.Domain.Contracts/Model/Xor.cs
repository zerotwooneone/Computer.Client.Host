namespace Computer.Client.Domain.Contracts.Model;

public record Xor<TLeft, TRight> 
{
    public TRight? Right { get; }
    public TLeft? Left { get; }

    public Xor(TLeft left)
    {
        Left = left;
    }
    
    public Xor(TRight right)
    {
        Right = right;
    }
}

public record XorResult<TLeft, TRight> : Xor<TLeft, TRight>, IResult 
    where TLeft : IResult 
    where TRight : IResult
{
    public bool Success => ((IResult?)Right ?? Left)?.Success ?? false;
    public XorResult(TLeft left) : base(left)
    {
    }
    
    public XorResult(TRight right) : base(right)
    {
    }
}

public record TypedResult<T> : XorResult<SuccessResult<T>, ErrorResult>
{
    public TypedResult(T value) : base(new SuccessResult<T>(value))
    {
        
    }

    public TypedResult(string reason) : base(new ErrorResult(reason))
    {
        
    }
}

public interface IResult
{
    bool Success { get; }
}

public record SuccessResult : IResult
{
    public bool Success => true;
    private SuccessResult(){}
    public static SuccessResult Instance = new SuccessResult();
}

public record SuccessResult<T>(T Value) : IResult
{
    public bool Success => true;
}

public record ErrorResult(string Reason) : IResult
{
    public bool Success => false;
}