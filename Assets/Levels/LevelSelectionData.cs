using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(menuName = "Hang Glider/Create LevelSelectionData")]
    public class LevelSelectionData : ScriptableObject
    {
        [field: SerializeField]
        public List<LevelData> Levels { get; private set; }
    }
}