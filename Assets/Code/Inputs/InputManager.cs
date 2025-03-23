using Code.Utility;
using UnityEngine;

namespace Code.Inputs
{
    public class InputManager : MonoBehaviour
    {
        public Inputs CurrentInputs { get; private set; }

        public event ValueChangedHandler<Inputs> InputsChanged;

        public void Invoke(Inputs value)
        {
            CurrentInputs = value;

            InputsChanged?.Invoke(value);
        }
    }
}