using Core.Management;
using UnityEngine;

namespace Core.Inputs
{
    public class InputDiffToRotationMap : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve rollAngleCurve = AnimationCurve.Linear(0, 0, 1, 90);

        private InputManager _inputManager;

        private void Start()
        {
            _inputManager = Locator.Instance.InputManager;
        }

        public float Evaluate(Inputs inputs)
        {
            var inputDiff = inputs.GetDiff() / (_inputManager.PlayerWeight * .5f);

            return Mathf.Sign(inputDiff) * rollAngleCurve.Evaluate(Mathf.Abs(inputDiff));
        }
    }
}