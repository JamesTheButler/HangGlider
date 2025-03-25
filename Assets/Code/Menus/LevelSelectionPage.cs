using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Menus
{
    public class LevelSelectionPage : MonoBehaviour
    {
        [SerializeField]
        private UIInputManager uiInputs;

        [SerializeField]
        private LevelSelectionData levelSelectionData;

        [SerializeField]
        private GameObject rootElement;

        [SerializeField]
        private GameObject levelButtonPrefab;

        private int _currentLevelIndex;
        private readonly List<LevelButton> _levelButtons = new();

        private void Start()
        {
            foreach (var level in levelSelectionData.Levels) AddLevelButton(level);

            uiInputs.LeftClicked += LeftClicked;
            uiInputs.RightClicked += RightClicked;
            uiInputs.ConfirmClicked += ConfirmClicked;

            Select(_currentLevelIndex);
        }

        public string GetSelectedLevel()
        {
            return levelSelectionData.Levels[_currentLevelIndex].Scene;
        }

        private void AddLevelButton(LevelData level)
        {
            var newButtonObject = Instantiate(levelButtonPrefab, rootElement.transform);
            var newButton = newButtonObject.GetComponent<LevelButton>();
            newButton.Setup(level);

            _levelButtons.Add(newButton);
        }

        private void Select(int levelIndex)
        {
            if (_currentLevelIndex == levelIndex)
            {
                return;
            }

            var oldButton = _levelButtons.ElementAt(_currentLevelIndex);
            oldButton.SetHighlighted(false);

            var newPair = _levelButtons[levelIndex];
            newPair.SetHighlighted(true);
            _currentLevelIndex = levelIndex;
        }

        private void OnDisable()
        {
            uiInputs.LeftClicked -= LeftClicked;
            uiInputs.RightClicked -= RightClicked;
            uiInputs.ConfirmClicked -= ConfirmClicked;
        }

        private void ConfirmClicked()
        {
            var nextLevelData = levelSelectionData.Levels[_currentLevelIndex];
            Debug.Log($"Loading level '{nextLevelData.Name}'...");
            Locator.Instance.GameManager.LoadLevel(nextLevelData.Scene);
        }

        private void RightClicked()
        {
            var newIndex = _currentLevelIndex + 1;
            if (newIndex >= _levelButtons.Count)
            {
                return;
            }

            Select(newIndex);
        }

        private void LeftClicked()
        {
            var newIndex = _currentLevelIndex - 1;
            if (newIndex < 0)
            {
                return;
            }

            Select(newIndex);
        }
    }
}