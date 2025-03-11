using Unity.Mathematics.Geometry;
using UnityEngine;

namespace Code
{
    public class GliderController : MonoBehaviour
    {
        [SerializeField] private float defaultMoveSpeedInSec = 5f;
        [SerializeField] private float maxRollAngleInDeg = 45f;
        [SerializeField] private float yawSpeedInDegPerSec = 2f;
        [SerializeField] private float rollSpeedInDegPerSec = 2f;
        [SerializeField] private float boostMultiplier = 2f;

        private FlightInputs _flightInputs;

        private Inputs _inputs = new(0, 0);
        private bool _isBoosting;

        private void Start()
        {
            SetUpFlightControls();
        }

        private void FixedUpdate()
        {
            var moveSpeed = _isBoosting ? defaultMoveSpeedInSec * boostMultiplier : defaultMoveSpeedInSec;
            transform.position += transform.forward * (moveSpeed * Time.deltaTime);

            ApplySteer();
        }

        private void OnDestroy()
        {
            _flightInputs.InputsChanged -= OnFlightInputsOnInputsChanged;
            _flightInputs.BoostChanged -= OnFlightInputsOnBoostChanged;
        }

        private void SetUpFlightControls()
        {
            _flightInputs = GetComponent<FlightInputs>();
            _inputs = _flightInputs.Inputs;
            _flightInputs.InputsChanged += OnFlightInputsOnInputsChanged;
            _flightInputs.BoostChanged += OnFlightInputsOnBoostChanged;
        }

        private void OnFlightInputsOnBoostChanged(bool boost)
        {
            _isBoosting = boost;
        }

        private void OnFlightInputsOnInputsChanged(Inputs inputs)
        {
            _inputs = inputs;
        }

        private void ApplySteer()
        {
            var inputDiff = ComputeInputDiff();

            var yaw = yawSpeedInDegPerSec * Time.deltaTime * -inputDiff;
            transform.RotateAround(
                transform.position,
                Vector3.up,
                yaw);

            var baseRoll = rollSpeedInDegPerSec * Time.deltaTime;

            var rollAngle = inputDiff * baseRoll;
            rollAngle = ClampRollAngle(rollAngle);

            // apply auto-leveling/auto un-roll
            if (Mathf.Approximately(rollAngle, 0f))
            {
                rollAngle = DisableAutoBalanceWobble(baseRoll);
            }

            transform.RotateAround(
                transform.position,
                transform.forward,
                rollAngle);
        }

        private float DisableAutoBalanceWobble(float baseRoll)
        {
            float rollAngle;
            var currentRoll = transform.localRotation.eulerAngles.z;
            var currentNormalizedRoll = NormalizeAngle(currentRoll);

            rollAngle = baseRoll * -Mathf.Sign(currentNormalizedRoll);
            // fix wobble around center
            if (
                (currentNormalizedRoll > 0 &&
                 currentNormalizedRoll < baseRoll &&
                 Mathf.Abs(rollAngle) > currentNormalizedRoll &&
                 rollAngle < 0f)
                ||
                (currentNormalizedRoll < 0 &&
                 Mathf.Abs(currentNormalizedRoll) < baseRoll &&
                 Mathf.Abs(rollAngle) > currentNormalizedRoll &&
                 rollAngle > 0f)
            )
            {
                rollAngle = -currentNormalizedRoll;
            }

            return rollAngle;
        }

        /// <summary>
        /// Clamp desired roll angle if it would over-rotate.
        /// </summary>
        private float ClampRollAngle(float rollAngle)
        {
            var currentRoll = transform.localRotation.eulerAngles.z;
            var currentNormalizedRoll = NormalizeAngle(currentRoll);

            if ((Mathf.Approximately(currentNormalizedRoll, maxRollAngleInDeg) && rollAngle > 0) ||
                (Mathf.Approximately(currentNormalizedRoll, -maxRollAngleInDeg) && rollAngle < 0))
            {
                return 0;
            }

            // if currentRoll == 44 and rollAngle == 2 => rollAngle = 1
            if (currentNormalizedRoll + rollAngle > maxRollAngleInDeg)
            {
                return maxRollAngleInDeg - currentNormalizedRoll;
            }
            
            // if currentRoll == -44 and rollAngle == -2 => rollAngle = -1
            if (currentNormalizedRoll + rollAngle < -maxRollAngleInDeg)
            {
                return -(maxRollAngleInDeg + currentNormalizedRoll);
            }

            return rollAngle;
        }

        private float ComputeInputDiff()
        {
            return _inputs.Right - _inputs.Left;
        }

        private static float NormalizeAngle(float angle)
        {
            while (angle > 180f) angle -= 360f;
            while (angle < -180f) angle += 360f;
            return angle;
        }
    }
}