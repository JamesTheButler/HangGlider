using UnityEngine;

namespace Code
{
    public class SerialFlightInputs : MonoBehaviour
    {
        private const float Scale = 10;
        public bool IsInverted = true;

        public event ValueChangedHandler<Inputs> InputsChanged;

        // Invoked when a line of data is received from the serial device.
        void OnMessageArrived(string msg)
        {
            if (!msg.Contains(','))
                return;
            Debug.Log(msg);

            var split = msg.Split(",");
            var left = float.Parse(split[0])/Scale;
            var right = float.Parse(split[1])/Scale;
            if (IsInverted)
            {
                InputsChanged?.Invoke(new Inputs(left, right));
            }
            else
            {
                InputsChanged?.Invoke(new Inputs(right, left));
            }
        }

        // Invoked when a connect/disconnect event occurs. The parameter 'success'
        // will be 'true' upon connection, and 'false' upon disconnection or
        // failure to connect.
        void OnConnectionEvent(bool success)
        {
            Debug.Log("connected?"+ success);
        }
    }
}