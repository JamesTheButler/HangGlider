using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class ConnectionIndicator : MonoBehaviour
    {
        [SerializeField, Required]
        private Image indicatorIcon;

        [SerializeField]
        private Color connectedColor;

        [SerializeField]
        private Color disconnectedColor;

        private void Start()
        {
            indicatorIcon.color = disconnectedColor;
        }

        public void OnConnectionStatusChanged(bool isConnected)
        {
            indicatorIcon.color = isConnected ? connectedColor : disconnectedColor;
        }
    }
}