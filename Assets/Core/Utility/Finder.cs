using Features.Player;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.Utility
{
    public static class Finder
    {
        [CanBeNull]
        public static GameObject Player => GameObject.FindGameObjectWithTag(Tags.Player);

        [CanBeNull]
        public static GliderController PlayerController => Player?.GetComponent<GliderController>();
    }
}