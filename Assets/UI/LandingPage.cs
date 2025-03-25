using Core.Inputs;
using Core.Management;
using UnityEngine;

namespace UI
{
    public class LandingPage : PageBase
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