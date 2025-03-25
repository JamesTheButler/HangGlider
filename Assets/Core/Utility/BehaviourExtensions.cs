using UnityEngine;

namespace Core.Utility
{
    public static class BehaviourExtensions
    {
        public static void SetEnabled(this Behaviour self, bool enabled)
        {
            self.enabled = enabled;
        }
    }
}