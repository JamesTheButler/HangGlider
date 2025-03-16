using UnityEngine;

namespace Code.Inputs
{
    public class SerialFlightInputs : MonoBehaviour
    {
        private const float Scale = 10;

        [SerializeField]
        private bool useFakeInputs;

        public bool isInverted = true;

        private int _i;

        private void Update()
        {
            if (useFakeInputs)
            {
                FakeInputs();
            }
        }

        private void FakeInputs()
        {
            _i++;
            if (_i > 100)
            {
                _i %= 100;
            }

            var move = _i / 100;

            InputsChanged?.Invoke(new Inputs(-move, move));
        }

        public event ValueChangedHandler<Inputs> InputsChanged;

        // Invoked when a line of data is received from the serial device.
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