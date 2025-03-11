namespace Code
{
    public record Inputs(float Left, float Right)
    {
        public bool IsNull => Left == 0 && Right == 0;
    }
}