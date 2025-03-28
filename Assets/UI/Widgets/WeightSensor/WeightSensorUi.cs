using Core.Inputs;
using NaughtyAttributes;
using UnityEngine;

namespace UI.Widgets.WeightSensor
{
    public class WeightSensorUi : MonoBehaviour
    {
        [Required, SerializeField]
        private InputManager inputManager;

        [Header("UIs"), Required, SerializeField]
        private WeightSensorEntry left;

        [Required, SerializeField]
        private WeightSensorEntry right;

        [Required, SerializeField]
        private WeightSensorEntry total;

        private void Start()
        {
            inputManager.InputsChanged += UpdateUI;
        }

        private void UpdateUI(Inputs inputs)
        {
            var totals = Mathf.Abs(inputs.Right) + Mathf.Abs(inputs.Left);

            left.SetKg(inputs.Left);
            var leftPercent = inputs.Left / totals * 100f;
            left.SetPercent(float.IsNaN(leftPercent) ? 0f : leftPercent);

            right.SetKg(inputs.Right);
            var rightPercent = inputs.Right / totals * 100f;
            right.SetPercent(float.IsNaN(rightPercent) ? 0f : rightPercent);

            total.SetKg(totals);
            var totalsPercent = totals > 0 ? 100f : 0f;
            total.SetPercent(totalsPercent);
        }
    }
}