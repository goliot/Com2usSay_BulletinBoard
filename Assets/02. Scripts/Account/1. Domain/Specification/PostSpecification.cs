public class PostSpecification : ISpecification<string>
{
    public string ErrorMessage { get; private set; }

    public bool IsSatisfiedBy(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
        {
            ErrorMessage = "가 비었습니다.";
            return false;
        }

        return true;
    }
}