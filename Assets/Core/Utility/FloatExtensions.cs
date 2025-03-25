using UnityEngine;

namespace Core.Utility
{
    public static class FloatExtensions
    {
        public static bool IsApproximatelyZero(this float value)
        {
            return Mathf.Approximately(value, 0f);
        }

        /// <summary>
        ///  Normalizes an angle to the range [-180, 180] degrees.
        /// </summary>
        public static float NormalizeAngle(this float angle)
        {
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            return angle;
        }
    }
}