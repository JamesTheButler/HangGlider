using System;
using Core.Management;
using Core.Utility;
using UnityEngine;

namespace Core.Inputs
{
    public class UIInputManager : MonoBehaviour
    {
        public Action LeftClicked { get; set; }
        public Action RightClicked { get; set; }
        public Action ConfirmClicked { get; set; }
        public Action AnythingClicked { get; set; }

        private bool _rightSensorState;
        private bool _leftSensorState;
        private Inputs? _previousInputs;

        private void Start()
        {
            var inputManager = Locator.Instance.InputManager;

            inputManager.InputsChanged += OnInputsChanged;
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
            // if the previous step was 
            return IsBelowThreshold(previousAxis) && !IsBelowThreshold(newAxis);
        }

        private bool IsBelowThreshold(float inputAxis)
        {
            return inputAxis.IsApproximatelyZero();
        }
    }
}