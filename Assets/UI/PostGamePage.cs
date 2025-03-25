using System.Collections;
using Core.Inputs;
using Core.Management;
using UnityEngine;

namespace UI
{
    public class PostGamePage : PageBase
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