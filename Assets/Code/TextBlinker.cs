using System.Collections;
using TMPro;
using UnityEngine;

namespace Code
{
    public class TextBlinker : MonoBehaviour
    {
        [SerializeField]
        private float blinkInterval = 1.5f;

        private TMP_Text _text;

        private void Start()
        {
            _text = gameObject.GetComponent<TMP_Text>();

            StartCoroutine(FadeInOut());
        }

        private IEnumerator FadeInOut()
        {
            while (true)
            {
                // Fade In
                yield return StartCoroutine(FadeTextTo(1f, blinkInterval));
                yield return new WaitForSeconds(0.5f);

                // Fade Out
                yield return StartCoroutine(FadeTextTo(0f, blinkInterval));
                yield return new WaitForSeconds(0.5f);
            }
        }

        private IEnumerator FadeTextTo(float targetAlpha, float duration)
        {
            var startAlpha = _text.alpha;
            float time = 0;

            while (time < duration)
            {
                _text.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            _text.alpha = targetAlpha;
        }
    }
}