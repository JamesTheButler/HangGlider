using System.Collections;
using UnityEngine;

namespace Code.Menus
{
    public class PostGamePage : Page
    {
        [SerializeField]
        private UIInputManager uiInputManager;

        [SerializeField]
        private float postGameResetTimeInSec;

        private Coroutine _postGameResetCoroutine;
        private GameManager _gameManager;

        protected override void OnOpen()
        {
            _gameManager = Locator.Instance.GameManager;
            uiInputManager.AnythingClicked += ResetGame;
            StartResetTimer();
        }

        protected override void OnClose()
        {
            uiInputManager.AnythingClicked -= ResetGame;
            _gameManager = null;
        }

        private void ResetGame()
        {
            if (_postGameResetCoroutine != null)
            {
                StopCoroutine(_postGameResetCoroutine);
                _postGameResetCoroutine = null;
            }

            _gameManager.ResetGame();
        }

        private void StartResetTimer()
        {
            if (_postGameResetCoroutine != null)
            {
                StopCoroutine(_postGameResetCoroutine);
            }

            _postGameResetCoroutine = StartCoroutine(ResetAfterDelay(postGameResetTimeInSec));
        }

        private IEnumerator ResetAfterDelay(float delayInSec)
        {
            yield return new WaitForSeconds(delayInSec);
            ResetGame();
        }
    }
}