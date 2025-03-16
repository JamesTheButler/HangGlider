using Code.Inputs;
using UnityEngine;

namespace Code
{
    public class GliderController : MonoBehaviour
    {
        [SerializeField]
        private KeyboardFlightInputs keyboardFlightInputs;

        [SerializeField]
        private SerialFlightInputs serialFlightInputs;

        [SerializeField]
        private float defaultMoveSpeedInSec = 5f;

        [SerializeField]
        private float yawSpeedInDegPerSec = 2f;

        [Header("Roll")]
        [SerializeField]
        private float rollSpeedInDegPerSec = 2f;

        [SerializeField]
        private float rollResetSpeedInDegPerSec = 2f;

        [SerializeField]
        private float maxRollAngleInDeg = 45f;

        private Inputs.Inputs _currentInputs = new(0, 0);

        private float CurrentRoll => transform.localRotation.eulerAngles.z.NormalizeAngle();

        private void Start()
        {
            SetUpFlightControls();
        }

        private void FixedUpdate()
        {
            transform.position += transform.forward * (defaultMoveSpeedInSec * Time.deltaTime);

            ApplyInputs();
        }

        private void OnDestroy()
        {
            keyboardFlightInputs.InputsChanged -= OnKeyboardFlightInputsOnInputsChanged;
            serialFlightInputs.InputsChanged -= OnKeyboardFlightInputsOnInputsChanged;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.LogError($"Collided with {other.tag}");
        }

        private void ApplyInputs()
        {
            var inputDiff = GetInputDiff();

            if (inputDiff.IsApproximatelyZero())
            {
                AutoLevel();
                return;
            }

            Steer(inputDiff);
        }

        private void AutoLevel()
        {
            var baseRoll = rollResetSpeedInDegPerSec * Time.deltaTime;
            var currentRoll = CurrentRoll;

            if (currentRoll.IsApproximatelyZero())
            {
                return;
            }

            var rollAngle = baseRoll * -Mathf.Sign(currentRoll);

            // fix wobble around center
            if (
                (currentRoll > 0 &&
                 currentRoll < baseRoll &&
                 Mathf.Abs(rollAngle) > currentRoll &&
                 rollAngle < 0f)
                ||
                (currentRoll < 0 &&
                 Mathf.Abs(currentRoll) < baseRoll &&
                 Mathf.Abs(rollAngle) > currentRoll &&
                 rollAngle > 0f)
            )
            {
                rollAngle = -currentRoll;
            }

            Roll(rollAngle);
        }

        private void Steer(float inputDiff)
        {
            // yaw
            var yaw = yawSpeedInDegPerSec * Time.deltaTime * -inputDiff;
            transform.RotateAround(
                transform.position,
                Vector3.up,
                yaw);

            // roll
            var rollAngle = inputDiff * rollSpeedInDegPerSec * Time.deltaTime;
            var clampedRollAngle = ClampRollAngle(rollAngle);
            Roll(clampedRollAngle);
        }

        private void Roll(float rollAngle)
        {
            transform.RotateAround(
                transform.position,
                transform.forward,
                rollAngle);
        }

        /// <summary>
        ///     Clamp desired roll angle if it would over-rotate.
        /// </summary>
        private float ClampRollAngle(float rollAngle)
        {
            var currentRoll = CurrentRoll;
            if (currentRoll + rollAngle > maxRollAngleInDeg)
            {
                rollAngle = maxRollAngleInDeg - currentRoll;
            }
            else if (currentRoll + rollAngle < -maxRollAngleInDeg)
            {
                rollAngle = -(maxRollAngleInDeg + currentRoll);
            }

            return rollAngle;
        }

        #region Input Detection

        private void SetUpFlightControls()
        {
            serialFlightInputs.InputsChanged += OnKeyboardFlightInputsOnInputsChanged;
            keyboardFlightInputs.InputsChanged += OnKeyboardFlightInputsOnInputsChanged;
            _currentInputs = keyboardFlightInputs.Inputs;
        }

        private void OnKeyboardFlightInputsOnInputsChanged(Inputs.Inputs inputs)
        {
            _currentInputs = inputs;
        }

        private float GetInputDiff()
        {
            return _currentInputs.Right - _currentInputs.Left;
        }

        #endregion
    }
}