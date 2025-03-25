using UnityEngine;

namespace Code.Menus
{
    public class LandingPage : Page
    {
        [SerializeField]
        private UIInputManager uiInputManager;

        private GameManager _gameManager;

        // back up manual input
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            _gameManager = Locator.Instance.GameManager;
            uiInputManager.AnythingClicked += StartGame;
        }

        protected override void OnClose()
        {
            base.OnClose();
            uiInputManager.AnythingClicked -= StartGame;
        }

        private void StartGame()
        {
            _gameManager.StartGame();
        }
    }
}