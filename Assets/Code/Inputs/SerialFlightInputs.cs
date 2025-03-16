using UnityEngine;

namespace Code.Inputs
{
    public class SerialFlightInputs : MonoBehaviour
    {
        private const float Scale = 10;

        public bool isInverted = true;

        private int _i;

        public event ValueChangedHandler<Inputs> InputsChanged;

        private void OnMessageArrived(string msg)
        {
            if (!msg.Contains(','))
            {
                return;
            }

            Debug.Log(msg);

            var split = msg.Split(",");
            var left = float.Parse(split[0]) / Scale;
            var right = float.Parse(split[1]) / Scale;
            if (isInverted)
            {
                InputsChanged?.Invoke(new Inputs(left, right));
            }
            else
            {
                InputsChanged?.Invoke(new Inputs(right, left));
            }
        }

        private void OnConnectionEvent(bool success)
        {
            Debug.Log("connected?" + success);
        }
    }
}