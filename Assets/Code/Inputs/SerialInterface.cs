using UnityEngine;
using UnityEngine.Events;

namespace Code.Inputs
{
    [RequireComponent(typeof(SerialController))]
    public class SerialInterface : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<bool> onConnectionStateChanged;

        [SerializeField]
        private UnityEvent<string> onMessageReceived;

        private bool _latestConnectionState;


        // magic method of SerialController package
        // ReSharper disable once UnusedMember.Local
        private void OnMessageArrived(string msg)
        {
            onMessageReceived?.Invoke(msg);
        }

        // magic method of SerialController package
        // ReSharper disable once UnusedMember.Local
        private void OnConnectionEvent(bool isConnected)
        {
            if (isConnected == _latestConnectionState)
            {
                return;
            }

            _latestConnectionState = isConnected;
            onConnectionStateChanged?.Invoke(isConnected);
        }
    }
}