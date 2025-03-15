namespace Code
{
    public record Inputs(float Left, float Right)
    {
        public bool IsNull => Left.IsApproximatelyZero() && Right.IsApproximatelyZero();
    }
}