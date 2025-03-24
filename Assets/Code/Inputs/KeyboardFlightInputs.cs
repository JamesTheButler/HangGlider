using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Inputs
{
    public class KeyboardFlightInputs : MonoBehaviour, InputActions.IFlightControlsActions
    {
        [SerializeField]
        private InputManager inputManager;

        private InputActions _controls;

        private Inputs Inputs { get; set; } = new(0, 0);

        public void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new InputActions();
                _controls.FlightControls.SetCallbacks(this);
            }

            _controls.FlightControls.Enable();
        }

        private void OnDisable()
        {
            _controls.FlightControls.Disable();
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