using UnityEngine;

namespace Code.Utility
{
    public static class BehaviourExtensions
    {
        public static void SetEnabled(this Behaviour self, bool enabled)
        {
            self.enabled = enabled;
        }
    }
}