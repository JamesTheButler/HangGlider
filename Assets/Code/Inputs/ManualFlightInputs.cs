using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Inputs
{
    public class ManualFlightInputs : MonoBehaviour, InputActions.IFlightControlsActions
    {
        [SerializeField]
        private InputManager inputManager;

        [SerializeField]
        private float simulatedWeightKg = 70f;

        [SerializeField]
        private float initialTickDelaySec = 2f;

        [SerializeField]
        private float tickRateMs = 30f;

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
            yield return new WaitForSeconds(initialTickDelaySec);

            var tickInterval = tickRateMs * .001f;
            while (true)
            {
                inputManager.Invoke(Inputs);
                yield return new WaitForSeconds(tickInterval);
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