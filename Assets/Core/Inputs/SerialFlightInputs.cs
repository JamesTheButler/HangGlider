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
            var left = float.Parse(split[0]) * weightAmplifier;
            var right = float.Parse(split[1]) * weightAmplifier;

            inputManager.Invoke(new Inputs(left, right));
        }

        public void OnConnectionStatusChanged(bool isConnected)
        {
            Debug.Log($"connection changed to '{isConnected}'");
        }
    }
}