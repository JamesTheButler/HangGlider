using Code.Utility;

namespace Code.Inputs
{
    public record Inputs(float Left, float Right)
    {
        public bool IsNull => Left.IsApproximatelyZero() && Right.IsApproximatelyZero();
    }
}