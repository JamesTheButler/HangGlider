using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Inputs;
using Core.Management;
using Core.Utility;
using Levels.Data;
using NaughtyAttributes;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UI.Pages.LevelSelection
{
    public class LevelSelectionPage : PageBase
    {
        [Required, SerializeField]
        private UIInputManager uiInputs;

        [Required, SerializeField]
        private GameManager gameManager;

        [Required, SerializeField]
        private LevelSelectionData levelSelectionData;

        [Required, SerializeField]
        private GameObject rootElement;

        [Required, SerializeField]
        private GameObject levelButtonPrefab;

        [SerializeField]
        private int inputBlockInMs;

        private int? _currentLevelIndex;
        private readonly List<LevelButton> _levelButtons = new();
        private Coroutine _inputBlockCoroutine;

        private void Awake()
        {
            foreach (var level in levelSelectionData.Levels)
            {
                AddLevelButton(level);
            }
        }

        // back-up input
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ConfirmClicked();
        }


        protected override void OnOpen()
        {
            _inputBlockCoroutine = StartCoroutine(DelayInputActivation());

            // select first
            Select(0);
        }

        protected override void OnClose()
        {
            StopCoroutine(_inputBlockCoroutine);

            uiInputs.LeftClicked -= LeftClicked;
            uiInputs.RightClicked -= RightClicked;
            uiInputs.ConfirmClicked -= ConfirmClicked;
        }

        private IEnumerator DelayInputActivation()
        {
            yield return new WaitForMilliseconds(inputBlockInMs);

            uiInputs.LeftClicked += LeftClicked;
            uiInputs.RightClicked += RightClicked;
            uiInputs.ConfirmClicked += ConfirmClicked;
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
                return;

            if (_currentLevelIndex is not null)
            {
                var oldButton = _levelButtons.ElementAt(_currentLevelIndex.Value);
                oldButton.SetHighlighted(false);
            }

            var newPair = _levelButtons[levelIndex];
            newPair.SetHighlighted(true);
            _currentLevelIndex = levelIndex;
        }

        private void ConfirmClicked()
        {
            if (_currentLevelIndex is null)
                return;

            var nextLevelData = levelSelectionData.Levels[_currentLevelIndex.Value];
            Debug.Log($"Loading level '{nextLevelData.Name}'...");
            gameManager.LoadLevel(nextLevelData.Scene);
        }

        private void RightClicked()
        {
            var newIndex = (_currentLevelIndex ?? 0) + 1;
            if (newIndex >= _levelButtons.Count)
                return;

            Select(newIndex);
        }

        private void LeftClicked()
        {
            var newIndex = (_currentLevelIndex ?? 0) - 1;
            if (newIndex < 0)
                return;

            Select(newIndex);
        }
    }
}