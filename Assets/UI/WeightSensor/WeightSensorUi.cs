using Core.Inputs;
using Core.Management;
using UnityEngine;

namespace UI.WeightSensor
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

        private void UpdateUI(Inputs inputs)
        {
            var totals = Mathf.Abs(inputs.right) + Mathf.Abs(inputs.left);

            left.SetKg(inputs.left);
            var leftPercent = inputs.left / totals * 100f;
            left.SetPercent(float.IsNaN(leftPercent) ? 0f : leftPercent);

            right.SetKg(inputs.right);
            var rightPercent = inputs.right / totals * 100f;
            right.SetPercent(float.IsNaN(rightPercent) ? 0f : rightPercent);

            total.SetKg(totals);
            var totalsPercent = totals > 0 ? 100f : 0f;
            total.SetPercent(totalsPercent);
        }
    }
}