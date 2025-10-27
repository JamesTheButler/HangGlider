using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Inputs
{
    public class MySerialController : MonoBehaviour
    {
        [SerializeField] private UnityEvent<bool> connectionChanged;
        [SerializeField] private UnityEvent<string> messageReceived;
        [SerializeField] private int baudRate;
        [SerializeField] private int reconnectionDelay;

        private readonly Dictionary<string, Thread> _threads = new();
        private readonly Dictionary<string, SerialThreadLines> _serialThreads = new();

        private void Start()
        {
            var availablePorts = SerialPort.GetPortNames();

            foreach (var port in availablePorts)
            {
                var serialThread = new SerialThreadLines(port,
                    baudRate,
                    reconnectionDelay,
                    3,
                    false,
                    false,
                    false);
                var thread = new Thread(serialThread.RunForever);
                thread.Start();

                _threads.Add(port, thread);
                _serialThreads.Add(port, serialThread);
            }
        }

        private void Update()
        {
            foreach (var (portName, port) in _serialThreads)
            {
                ReadSerialMessageToMessageListener(portName, port);
            }
        }

        private void ReadSerialMessageToMessageListener(string portName, SerialThreadLines serialThread)
        {
            var message = (string)serialThread.ReadMessage();
            if (message == null)
                return;


            if (message.Contains(","))
            {
                Debug.Log($"message '{message}' on port {portName}");
                connectionChanged.Invoke(true);
                messageReceived.Invoke(message);
            }
        }

        private void SelectPort(string portName)
        {
            Debug.Log($"Selected Port {portName}. Stopping all other threads.");
            foreach (var (port, thread) in _serialThreads)
            {
                if (port == portName) continue;

                thread.RequestStop();
            }

            foreach (var (port, thread) in _threads)
            {
                if (port == portName) continue;

                thread.Abort();
            }
        }
    }
}