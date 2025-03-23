using System;
using TMPro;
using UnityEngine;

namespace Code.Menus
{
    public class InGameUIManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text timerText;

        private DateTime _startTime;

        private void Update()
        {
            var timeDiff = DateTime.Now - _startTime;

            timerText.text = $"{timeDiff.Minutes:D2}:{timeDiff.Seconds:D2}";
        }

        public void Initialize()
        {
            _startTime = DateTime.Now;
        }
    }
}