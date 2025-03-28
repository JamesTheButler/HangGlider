using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace UI.Widgets.WeightSensor
{
    public class WeightSensorEntry : MonoBehaviour
    {
        [Required, SerializeField]
        private TMP_Text kgText;

        [Required, SerializeField]
        private TMP_Text percentText;

        public void SetPercent(float percent)
        {
            percentText.text = percent.ToString("0.00");
        }

        public void SetKg(float kg)
        {
            kgText.text = kg.ToString("0.00");
        }
    }
}