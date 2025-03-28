using System;
using Core.Utility;
using UnityEngine;

namespace Core.Inputs
{
    [Serializable]
    public struct Inputs
    {
        [field: SerializeField]
        public float Left { get; private set; }

        [field: SerializeField]
        public float Right { get; private set; }

        public Inputs(float left, float right)
        {
            Left = left;
            Right = right;
        }

        public Inputs Copy(float? newLeft = null, float? newRight = null)
        {
            return new Inputs(newLeft ?? Left, newRight ?? Right);
        }

        public bool IsApproximatelyZero()
        {
            return Left.IsApproximatelyZero() && Right.IsApproximatelyZero();
        }
    }
}