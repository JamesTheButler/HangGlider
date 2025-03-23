using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Inputs
{
    public class KeyboardFlightInputs : MonoBehaviour, InputActions.IFlightControlsActions
    {
        [SerializeField]
        private InputManager inputManager;

        private InputActions _controls;

        public Inputs Inputs { get; private set; } = new(0, 0);

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

            inputManager.Invoke(Inputs);
        }

        public void OnLeft(InputAction.CallbackContext context)
        {
            Inputs = Inputs with { Left = context.ReadValue<float>() };
            inputManager.Invoke(Inputs);
        }

        public void OnBoost(InputAction.CallbackContext context)
        {
        }
    }
}