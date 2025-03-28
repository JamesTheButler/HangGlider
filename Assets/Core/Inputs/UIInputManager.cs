using System;
using Core.Management;
using Core.Utility;
using NaughtyAttributes;
using UnityEngine;

namespace Core.Inputs
{
    public class UIInputManager : MonoBehaviour
    {
        [SerializeField, Required]
        private InputManager inputManager;

        public Action LeftClicked { get; set; }
        public Action RightClicked { get; set; }
        public Action ConfirmClicked { get; set; }
        public Action AnythingClicked { get; set; }

        private bool _rightSensorState;
        private bool _leftSensorState;
        private Inputs? _previousInputs;

        private void OnEnable()
        {
            inputManager.InputsChanged += OnInputsChanged;
        }

        private void OnDisable()
        {
            inputManager.InputsChanged -= OnInputsChanged;
        }

        private void OnInputsChanged(Inputs newInputs)
        {
            var isLeftClicked = DetectClick(_previousInputs?.Left ?? 0f, newInputs.Left);
            var isRightClicked = DetectClick(_previousInputs?.Right ?? 0f, newInputs.Right);

            if (isLeftClicked && isRightClicked)
            {
                ConfirmClicked?.Invoke();
                AnythingClicked?.Invoke();
            }
            else if (isLeftClicked)
            {
                LeftClicked?.Invoke();
                AnythingClicked?.Invoke();
            }
            else if (isRightClicked)
            {
                RightClicked?.Invoke();
                AnythingClicked?.Invoke();
            }

            _previousInputs = newInputs;
        }

        private bool DetectClick(float previousAxis, float newAxis)
        {
            // if the previous axis value was above the threshold and the new value is below
            return !IsBelowThreshold(previousAxis) && IsBelowThreshold(newAxis);
        }

        private bool IsBelowThreshold(float inputAxis)
        {
            return inputAxis.IsApproximatelyZero();
        }
    }
}