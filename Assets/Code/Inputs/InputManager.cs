using Code.Utility;
using NaughtyAttributes;
using UnityEngine;

namespace Code.Inputs
{
    public class InputManager : MonoBehaviour
    {
        // TODO: this needs to be calculated somehow
        public float PlayerWeight { get; } = 70f;

        [field: SerializeField]
        [field: ReadOnly]
        public Inputs CurrentInputs { get; private set; }

        public event ValueChangedHandler<Inputs> InputsChanged;

        public void Invoke(Inputs value)
        {
            CurrentInputs = value;

            InputsChanged?.Invoke(value);
        }
    }
}