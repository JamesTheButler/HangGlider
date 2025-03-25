using System;
using UnityEngine;

namespace Code.Menus
{
    public class UIInputManager : MonoBehaviour
    {
        public Action LeftClicked { get; set; }
        public Action RightClicked { get; set; }
        public Action ConfirmClicked { get; set; }
        public Action AnythingClicked { get; set; }

        private void Start()
        {
            var inputManager = Locator.Instance.InputManager;

            inputManager.InputsChanged += OnInputsChanged;
        }

        private void OnInputsChanged(Inputs.Inputs newInputs)
        {
            // do processing here to invoke the actions
            AnythingClicked?.Invoke();
        }
    }
}