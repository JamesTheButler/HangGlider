using System.Globalization;
using UnityEngine;

namespace Core.Inputs
{
    public class SerialFlightInputs : MonoBehaviour
    {
        [SerializeField]
        private InputManager inputManager;

        [SerializeField]
        private float weightAmplifier;

        public void OnMessageArrived(string message)
        {
            Debug.Log(message);
            if (!message.Contains(','))
            {
                return;
            }

            var split = message.Split(",");
            var leftRaw = float.Parse(split[0], CultureInfo.InvariantCulture);  ; ;
            var rightRaw = float.Parse(split[1], CultureInfo.InvariantCulture);

            var left = leftRaw > 0.5 ? leftRaw * weightAmplifier : 0f;
            var right = rightRaw > 0.5 ? rightRaw * weightAmplifier : 0f;
            inputManager.Invoke(new Inputs(left, right));
        }

        public void OnConnectionStatusChanged(bool isConnected)
        {
            Debug.Log($"connection changed to '{isConnected}'");
        }
    }
}