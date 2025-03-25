using System;

namespace Code.Inputs
{
    [Serializable]
    public struct Inputs
    {
        public float left;
        public float right;

        public Inputs(float left, float right)
        {
            this.left = left;
            this.right = right;
        }

        public Inputs Copy(float? newLeft = null, float? newRight = null)
        {
            return new Inputs(newLeft ?? left, newRight ?? right);
        }
    }
}