using UnityEngine;

namespace Code
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField]
        private Transform parent;

        [SerializeField]
        private Transform spawnPoint;

        [SerializeField]
        private GameObject playerPrefab;

        private void Start()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation, parent ?? transform.parent);
        }
    }
}