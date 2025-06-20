public class CommentSpecification : ISpecification<string>
{
    public string ErrorMessage { get; private set; }

    public bool IsSatisfiedBy(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
        {
            ErrorMessage = "비어있을 수 없습니다.";
            return false;
        }

        return true;
    }
}