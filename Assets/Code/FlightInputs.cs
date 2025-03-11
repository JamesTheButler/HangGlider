using UnityEngine;
using UnityEngine.InputSystem;

namespace Code
{
    public class FlightInputs : MonoBehaviour, InputActions.IFlightControlsActions
    {
        public event ValueChangedHandler<Inputs> InputsChanged;
        public event ValueChangedHandler<bool> BoostChanged;

        public Inputs Inputs { get; private set; } = new(0, 0);

        private InputActions _controls;

        public void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new InputActions();
                _controls.FlightControls.SetCallbacks(this);
            }

            _controls.FlightControls.Enable();
        }

        public void OnRight(InputAction.CallbackContext context)
        {
            Inputs = Inputs with { Right = context.ReadValue<float>() };
            InputsChanged?.Invoke(Inputs);
        }

        public void OnLeft(InputAction.CallbackContext context)
        {
            Inputs = Inputs with { Left = context.ReadValue<float>() };
            InputsChanged?.Invoke(Inputs);
        }

        public void OnBoost(InputAction.CallbackContext context)
        {
            BoostChanged?.Invoke(context.ReadValueAsButton());
        }
    }
}