using NaughtyAttributes;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(menuName = "Hang Glider/Create LevelData")]
    public class LevelData : ScriptableObject
    {
        [field: Scene]
        [field: SerializeField]
        public string Scene { get; private set; }

        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public int Length { get; private set; }

        [field: SerializeField]
        public string Grade { get; private set; }
    }
}