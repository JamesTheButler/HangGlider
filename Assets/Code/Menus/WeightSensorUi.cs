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
            var totals = Mathf.Abs(inputs.Right) + Mathf.Abs(inputs.Left);

            left.SetKg(inputs.Left);
            var leftPercent = inputs.Left / totals * 100f;
            left.SetPercent(float.IsNaN(leftPercent) ? 0f : leftPercent);

            right.SetKg(inputs.Right);
            var rightPercent = inputs.Right / totals * 100f;
            right.SetPercent(float.IsNaN(rightPercent) ? 0f : rightPercent);

            total.SetKg(totals);
            total.SetPercent(totals / totals * 100f);
        }
    }
}