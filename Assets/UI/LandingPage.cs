using Core.Inputs;
using Core.Management;
using NaughtyAttributes;
using UnityEngine;

namespace UI
{
    public class LandingPage : PageBase
    {
        [Required, SerializeField]
        private UIInputManager uiInputManager;

        [Required, SerializeField]
        private GameManager gameManager;

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
            uiInputManager.AnythingClicked += StartGame;
        }

        protected override void OnClose()
        {
            base.OnClose();
            uiInputManager.AnythingClicked -= StartGame;
        }

        private void StartGame()
        {
            gameManager.StartGame();
        }
    }
}