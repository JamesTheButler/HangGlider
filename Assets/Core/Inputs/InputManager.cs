using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Core.Inputs
{
    public class InputManager : MonoBehaviour
    {
        // TODO: this needs to be calculated somehow
        public float PlayerWeight { get; } = 70f;

        [SerializeField]
        private float hangingChangeDelayInMs = 200;

        [field: SerializeField, ReadOnly, Foldout("Debugging")]
        public Inputs CurrentInputs { get; private set; }

        [field: SerializeField, ReadOnly, Foldout("Debugging")]
        public bool IsPlayerHanging { get; private set; }

        public Action<Inputs> InputsChanged { get; set; }
        public Action<bool> IsPlayerHangingChanged { get; set; }

        private Coroutine _stoppedHangingCheckCoroutine;
        private Coroutine _startedHangingCheckCoroutine;

        public void Invoke(Inputs newInputs)
        {
            ChangeCurrentInputs(newInputs);
            CheckIfIsHangingChanged(newInputs);
        }

        private void ChangeCurrentInputs(Inputs newInputs)
        {
            CurrentInputs = newInputs;
            InputsChanged?.Invoke(newInputs);
        }

        private void ChangeIsPlayerHanging(bool newIsPlayerHanging)
        {
            if (IsPlayerHanging == newIsPlayerHanging)
                return;

            IsPlayerHanging = newIsPlayerHanging;
            IsPlayerHangingChanged?.Invoke(newIsPlayerHanging);
            _startedHangingCheckCoroutine = null;
            _stoppedHangingCheckCoroutine = null;
        }

        private void CheckIfIsHangingChanged(Inputs newInputs)
        {
            // detect if we have to change the hanging state
            if (newInputs.IsApproximatelyZero())
            {
                // Cancel non-zero coroutine if it's running
                if (_startedHangingCheckCoroutine != null)
                {
                    StopCoroutine(_startedHangingCheckCoroutine);
                    _startedHangingCheckCoroutine = null;
                }

                // Start zero-check coroutine only if it's not already running
                _stoppedHangingCheckCoroutine ??= StartCoroutine(StoppedHanging());
            }
            else
            {
                // Cancel zero-check coroutine if it's running
                if (_stoppedHangingCheckCoroutine != null)
                {
                    StopCoroutine(_stoppedHangingCheckCoroutine);
                    _stoppedHangingCheckCoroutine = null;
                }

                // Start non-zero-check coroutine only if it's not already running
                _startedHangingCheckCoroutine ??= StartCoroutine(StartHanging());
            }
        }

        private IEnumerator StoppedHanging()
        {
            yield return new WaitForSeconds(hangingChangeDelayInMs * 0.001f);
            ChangeIsPlayerHanging(false);
        }

        private IEnumerator StartHanging()
        {
            yield return new WaitForSeconds(hangingChangeDelayInMs * 0.001f);
            ChangeIsPlayerHanging(true);
        }
    }
}