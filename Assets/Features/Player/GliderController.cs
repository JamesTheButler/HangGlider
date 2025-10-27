using Core.Inputs;
using Core.Management;
using Core.Utility;
using UnityEngine;

namespace Features.Player
{
    public class GliderController : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float centerThreshold = 0.1f, maxThreshold = 0.75f;

        [SerializeField] private float defaultMoveSpeedInSec = 5f;

        [SerializeField] private float yawSpeedInDegPerSec = 2f;

        [Header("Roll"), SerializeField] private float rollSpeedInDegPerSec = 2f;

        [SerializeField] private float rollResetSpeedInDegPerSec = 2f;

        [SerializeField] private float maxRollAngleInDeg = 45f;

        private bool _isFlying;

        private InputManager _inputManager;
        private InputDiffToRotationMap _inputMap;
        private GameManager _gameManager;
        private Inputs _currentInputs = new(0, 0);

        private float CurrentRoll => transform.localRotation.eulerAngles.z.NormalizeAngle();

        #region Lifecycle Methods

        private void Start()
        {
            _inputManager = Locator.Instance.InputManager;
            _inputMap = Locator.Instance.InputMap;
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
            if (_inputManager.PlayerWeight.IsApproximatelyZero())
                return;
            
            //var inputDiff = _inputMap.Evaluate(_currentInputs);
            var inputDiff = (_currentInputs.Left - _currentInputs.Right) / _inputManager.PlayerWeight;
            
            Steer(inputDiff);
        }

        private void Steer(float inputDiff)
        {
            var absDiff = Mathf.Abs(inputDiff);
            // yaw
            var yawDelta = yawSpeedInDegPerSec * Time.deltaTime * -inputDiff;
            transform.RotateAround(
                transform.position,
                Vector3.up,
                yawDelta);

            // roll
            float rollAngle;

            if (absDiff < centerThreshold)
            {
                rollAngle = 0f;
            }
            else if (absDiff > maxThreshold)
            {
                rollAngle = maxRollAngleInDeg;
            }
            else
            {
                var t = Mathf.InverseLerp(centerThreshold, maxThreshold, absDiff);
                rollAngle = Mathf.Lerp(0, maxRollAngleInDeg, t);
            }

            rollAngle *= Mathf.Sign(inputDiff);


            var pitch = transform.eulerAngles.x; // preserve pitch
            var yaw = transform.eulerAngles.y; // preserve pitch
            transform.rotation = Quaternion.Euler(pitch, yaw, rollAngle);
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
            Debug.Log($"new input {inputs}");
            _currentInputs = inputs;
        }

        #endregion
    }
}