using Code.Inputs;
using Code.Menus;
using UnityEngine;

namespace Code
{
    public class Locator : MonoBehaviour
    {
        public static Locator Instance { get; private set; }

        [field: SerializeField]
        public GameManager GameManager { get; private set; }

        [field: SerializeField]
        public UIManager UIManager { get; private set; }

        [field: SerializeField]
        public InputManager InputManager { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}