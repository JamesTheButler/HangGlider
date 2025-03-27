using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InGameUi : PageBase
    {
        [Required, SerializeField]
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