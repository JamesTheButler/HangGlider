using Code.Inputs;
using UnityEngine;

namespace Code.Menus
{
    public class WeightSensorUi : MonoBehaviour
    {
        private InputManager _inputManager;

        [Header("UIs")]
        [SerializeField]
        private WeightSensorEntry left;

        [SerializeField]
        private WeightSensorEntry right;

        [SerializeField]
        private WeightSensorEntry total;

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