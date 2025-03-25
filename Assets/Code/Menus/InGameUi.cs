using System;
using TMPro;
using UnityEngine;

namespace Code.Menus
{
    public class InGameUi : Page
    {
        [SerializeField]
        private TMP_Text timerText;

        private DateTime _startTime;

        private void Update()
        {
            var timeDiff = DateTime.Now - _startTime;

            timerText.text = $"{timeDiff.Minutes:D2}:{timeDiff.Seconds:D2}";
        }

        protected override void OnOpen()
        {
            _startTime = DateTime.Now;
        }
    }
}