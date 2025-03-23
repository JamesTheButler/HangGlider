using Code.Inputs;
using UnityEngine;

namespace Code.Menus
{
    public class WeightDebugUi : MonoBehaviour
    {
        private InputManager _inputManager;

        [Header("UIs")]
        [SerializeField]
        private WeightDebuggerEntry left;

        [SerializeField]
        private WeightDebuggerEntry right;

        [SerializeField]
        private WeightDebuggerEntry total;

        private void Start()
        {
            _inputManager = Locator.Instance.InputManager;
            _inputManager.InputsChanged += UpdateUI;
        }

        private void UpdateUI(Inputs.Inputs inputs)
        {
            left.SetKg(inputs.Left);
            left.SetPercent(inputs.Left);

            right.SetKg(inputs.Right);
            right.SetPercent(inputs.Right);

            total.SetKg(inputs.Right - inputs.Left);
            total.SetPercent(inputs.Right - inputs.Left);
        }
    }
}