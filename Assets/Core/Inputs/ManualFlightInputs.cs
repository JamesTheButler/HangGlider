using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Inputs
{
    public class ManualFlightInputs : MonoBehaviour, InputActions.IFlightControlsActions
    {
        [SerializeField]
        private InputManager inputManager;

        [SerializeField]
        private float simulatedWeightKg = 70f;

        [SerializeField]
        private int tickRateMs = 30;

        private InputActions _controls;

        private Inputs Inputs { get; set; } = new(0, 0);

        private Coroutine _tickCoroutine;

        public void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new InputActions();
                _controls.FlightControls.SetCallbacks(this);
            }

            _controls.FlightControls.Enable();

            _tickCoroutine = StartCoroutine(PostInputs());
        }

        private void OnDisable()
        {
            _controls.FlightControls.Disable();

            if (_tickCoroutine != null)
            {
                StopCoroutine(_tickCoroutine);
                _tickCoroutine = null;
            }
        }

        private IEnumerator PostInputs()
        {
            while (true)
            {
                if (isActiveAndEnabled)
                { 
                    inputManager.Invoke(Inputs);
                }
                yield return new Utility.WaitForMilliseconds(tickRateMs);
            }
        }

        public void OnRight(InputAction.CallbackContext context)
        {
            var inputAxis = context.ReadValue<float>();
            Inputs = Inputs.Copy(newRight: RemapInput(inputAxis));
        }

        public void OnLeft(InputAction.CallbackContext context)
        {
            var inputAxis = context.ReadValue<float>();
            Inputs = Inputs.Copy(RemapInput(inputAxis));
        }

        // 1 => 0
        // 0 => half weight
        // -1 => full weight
        private float RemapInput(float inputAxis)
        {
            var halfWeight = simulatedWeightKg * 0.5f;
            var invertedAxis = inputAxis * -1;
            return (invertedAxis + 1) * halfWeight;
        }

        public void OnBoost(InputAction.CallbackContext context)
        {
        }
    }
}