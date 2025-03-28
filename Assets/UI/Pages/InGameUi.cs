using System;
using System.Collections;
using Core.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace UI.Pages
{
    public class InGameUi : PageBase
    {
        [Required, SerializeField]
        private TMP_Text timerText;

        [Required, SerializeField]
        private TMP_Text countDownText;

        private DateTime _startTime;
        private Coroutine _countDownCoroutine;

        private void Update()
        {
            var timeDiff = DateTime.Now - _startTime;

            timerText.text = $"{timeDiff.Minutes:D2}:{timeDiff.Seconds:D2}";
        }

        protected override void OnOpen()
        {
            _startTime = DateTime.Now;

            _countDownCoroutine = StartCoroutine(CountDown());
        }

        protected override void OnClose()
        {
            if (_countDownCoroutine != null)
            {
                StopCoroutine(_countDownCoroutine);
                _countDownCoroutine = null;
                // just in case
                countDownText.gameObject.SetActive(false);
            }

            base.OnClose();
        }

        private IEnumerator CountDown()
        {
            countDownText.gameObject.SetActive(true);
            countDownText.text = "3";
            yield return new WaitForSeconds(1);
            countDownText.text = "2";
            yield return new WaitForSeconds(1);
            countDownText.text = "1";
            yield return new WaitForSeconds(1);
            countDownText.text = "GO!";
            Finder.PlayerController?.StartFlight();
            yield return new WaitForSeconds(1);
            countDownText.gameObject.SetActive(false);
        }
    }
}