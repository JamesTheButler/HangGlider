using UnityEngine;

namespace Code
{
    public class GliderController : MonoBehaviour
    {
        [SerializeField]
        private FlightInputs flightInputs;

        [SerializeField]
        private float defaultMoveSpeedInSec = 5f;

        [SerializeField]
        private float yawSpeedInDegPerSec = 2f;

        [SerializeField]
        private float boostMultiplier = 2f;

        [Header("Roll")]
        [SerializeField]
        private float rollSpeedInDegPerSec = 2f;

        [SerializeField]
        private float rollResetSpeedInDegPerSec = 2f;

        [SerializeField]
        private float maxRollAngleInDeg = 45f;

        private Inputs _currentInputs = new(0, 0);
        private bool _isBoosting;

        private int i;

        private float CurrentRoll => transform.localRotation.eulerAngles.z.NormalizeAngle();

        private void Start()
        {
            SetUpFlightControls();
        }

        private void FixedUpdate()
        {
            var moveSpeed = _isBoosting ? defaultMoveSpeedInSec * boostMultiplier : defaultMoveSpeedInSec;
            transform.position += transform.forward * (moveSpeed * Time.deltaTime);

            ApplyInputs();
        }

        private void OnDestroy()
        {
            flightInputs.InputsChanged -= OnFlightInputsOnInputsChanged;
            flightInputs.BoostChanged -= OnFlightInputsOnBoostChanged;
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
            i++;
            // yaw
            var yaw = yawSpeedInDegPerSec * Time.deltaTime * -inputDiff;
            transform.RotateAround(
                transform.position,
                Vector3.up,
                yaw);

            // roll
            var rollAngle = inputDiff * rollSpeedInDegPerSec * Time.deltaTime;
            var clampedRollAngle = ClampRollAngle(rollAngle);

            if (Mathf.Abs(CurrentRoll) > 40)
            {
                Debug.Log($"[{i}] current {CurrentRoll} + {clampedRollAngle} (initial: {rollAngle})");
            }

            Roll(clampedRollAngle);

            if (Mathf.Abs(CurrentRoll) > 40)
            {
                Debug.Log($"[{i}] new roll {CurrentRoll}");
            }
            // clamp roll
            //var clampedRoll = Mathf.Clamp(CurrentRoll, -maxRollAngleInDeg, maxRollAngleInDeg);
            //var currentRot = transform.localRotation;
            //transform.localRotation = Quaternion.Euler(currentRot.x, currentRot.y, clampedRoll);
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
            _currentInputs = flightInputs.Inputs;
            flightInputs.InputsChanged += OnFlightInputsOnInputsChanged;
            flightInputs.BoostChanged += OnFlightInputsOnBoostChanged;
        }

        private void OnFlightInputsOnBoostChanged(bool boost)
        {
            _isBoosting = boost;
        }

        private void OnFlightInputsOnInputsChanged(Inputs inputs)
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