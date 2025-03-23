using UnityEngine;

namespace Code.Inputs
{
    public class SerialFlightInputs : MonoBehaviour
    {
        [SerializeField]
        private InputManager inputManager;

        private const float Scale = 10;

        public bool isInverted = true;

        private int _i;

        public void OnMessageArrived(string message)
        {
            if (!message.Contains(','))
            {
                return;
            }

            Debug.Log(message);

            var split = message.Split(",");
            var left = float.Parse(split[0]) / Scale;
            var right = float.Parse(split[1]) / Scale;
            if (isInverted)
            {
                inputManager.Invoke(new Inputs(left, right));
            }
            else
            {
                inputManager.Invoke(new Inputs(right, left));
            }
        }

        public void OnConnectionStatusChanged(bool isConnected)
        {
            Debug.Log("connection changed?" + isConnected);
        }
    }
}