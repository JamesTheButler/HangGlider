using System.Collections;
using Core.Inputs;
using Core.Management;
using Core.Utility;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Pages
{
    public class PostGamePage : PageBase
    {
        [Required, SerializeField]
        private UIInputManager uiInputManager;

        [Required, SerializeField]
        private GameManager gameManager;

        [SerializeField]
        private int inputBlockInMs = 3000;

        [SerializeField]
        private float resetTime;

        private Coroutine _postGameResetCoroutine;
        private Coroutine _inputBlockCoroutine;

        protected override void OnOpen()
        {
            _inputBlockCoroutine = StartCoroutine(DelayInputActivation());
        }

        protected override void OnClose()
        {
            if (_inputBlockCoroutine != null)
                StopCoroutine(_inputBlockCoroutine);

            if (_postGameResetCoroutine != null)
                StopCoroutine(_postGameResetCoroutine);

            uiInputManager.AnythingClicked -= ResetGame;
        }


        private IEnumerator DelayInputActivation()
        {
            yield return new WaitForMilliseconds(inputBlockInMs);

            uiInputManager.AnythingClicked += ResetGame;
            StartResetTimer();
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
                StopCoroutine(_postGameResetCoroutine);

            _postGameResetCoroutine = StartCoroutine(ResetAfterDelay(resetTime));
        }

        private IEnumerator ResetAfterDelay(float delayInSec)
        {
            yield return new WaitForSeconds(delayInSec);
            ResetGame();
        }
    }
}