using Levels.Data;
using TMPro;
using UnityEngine;

namespace UI.LevelSelection
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField]
        private GameObject highlightFrame;

        [SerializeField]
        private TMP_Text nameText;

        [SerializeField]
        private TMP_Text gradeText;

        [SerializeField]
        private TMP_Text durationText;

        public void Setup(LevelData levelData)
        {
            nameText.text = levelData.Name;
            gradeText.text = $"Grade: {levelData.Grade}";
            durationText.text = $"Length: {levelData.Length}";
            highlightFrame.SetActive(false);
        }

        public void SetHighlighted(bool isHighlighted)
        {
            highlightFrame.SetActive(isHighlighted);
        }
    }
}