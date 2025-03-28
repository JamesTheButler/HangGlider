using Core.Inputs;
using Core.Management;
using Core.Utility;
using UnityEngine;

namespace Features.Player
{
    public class GliderController : MonoBehaviour
    {
        [SerializeField]
        private float defaultMoveSpeedInSec = 5f;

        [SerializeField]
        private float yawSpeedInDegPerSec = 2f;

        [Header("Roll"), SerializeField]
        private float rollSpeedInDegPerSec = 2f;

        [SerializeField]
        private float rollResetSpeedInDegPerSec = 2f;

        [SerializeField]
        private float maxRollAngleInDeg = 45f;

        private bool _isFlying;

        private InputManager _inputManager;
        private GameManager _gameManager;
        private Inputs _currentInputs = new(0, 0);

        private float CurrentRoll => transform.localRotation.eulerAngles.z.NormalizeAngle();

        #region Lifecycle Methods

        private void Start()
        {
            _inputManager = Locator.Instance.InputManager;
            _gameManager = Locator.Instance.GameManager;
        }

        private void FixedUpdate()
        {
            if (!enabled || !_isFlying)
                return;

            transform.position += transform.forward * (defaultMoveSpeedInSec * Time.deltaTime);

            ApplyInputs();
        }

        private void OnDestroy()
        {
            _inputManager.InputsChanged -= OnInputsChanged;
            _inputManager.IsPlayerHangingChanged -= OnPlayerHangingChanged;
        }

        public void StartFlight()
        {
            SetUpFlightControls();
            _isFlying = true;
        }

        private void OnPlayerHangingChanged(bool isHanging)
        {
            if (isHanging) return;

            _gameManager.PlayerLetGo();
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case Tags.Environment:
                    _gameManager.CollisionWithObstacle();
                    break;
                case Tags.Goal:
                    _gameManager.CollisionWithGoal();
                    break;
            }
        }

        #endregion

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
                return;

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
                rollAngle = -currentRoll;

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
        /// Clamp desired roll angle if it would over-rotate.
        /// </summary>
        private float ClampRollAngle(float rollAngle)
        {
            var currentRoll = CurrentRoll;
            if (currentRoll + rollAngle > maxRollAngleInDeg)
                rollAngle = maxRollAngleInDeg - currentRoll;
            else if (currentRoll + rollAngle < -maxRollAngleInDeg)
                rollAngle = -(maxRollAngleInDeg + currentRoll);

            return rollAngle;
        }

        #region Input Detection

        private void SetUpFlightControls()
        {
            OnInputsChanged(_inputManager.CurrentInputs);
            _inputManager.InputsChanged += OnInputsChanged;
            _inputManager.IsPlayerHangingChanged += OnPlayerHangingChanged;
        }

        private void OnInputsChanged(Inputs inputs)
        {
            _currentInputs = inputs;
        }

        private float GetInputDiff()
        {
            return (_currentInputs.Left - _currentInputs.Right) / (_inputManager.PlayerWeight * .5f);
        }

        #endregion
    }
}