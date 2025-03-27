using System.Collections;
using Core.Inputs;
using Core.Management;
using NaughtyAttributes;
using UnityEngine;

namespace UI
{
    public class PostGamePage : PageBase
    {
        [Required, SerializeField]
        private UIInputManager uiInputManager;

        [Required, SerializeField]
        private GameManager gameManager;

        [SerializeField]
        private float postGameResetTimeInSec;

        private Coroutine _postGameResetCoroutine;

        protected override void OnOpen()
        {
            uiInputManager.AnythingClicked += ResetGame;
            StartResetTimer();
        }

        protected override void OnClose()
        {
            uiInputManager.AnythingClicked -= ResetGame;
            gameManager = null;
        }

        private void ResetGame()
        {
            if (_postGameResetCoroutine != null)
            {
                StopCoroutine(_postGameResetCoroutine);
                _postGameResetCoroutine = null;
            }

            gameManager.ResetGame();
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