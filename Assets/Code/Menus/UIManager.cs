using System.Collections.Generic;
using Code.Inputs;
using UnityEngine;

namespace Code.Menus
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Pages")]
        [SerializeField]
        private GameObject landingPage;

        [SerializeField]
        private GameObject levelSelectionPage;

        [SerializeField]
        private GameObject inGameUi;

        [SerializeField]
        private GameObject levelFailedUi;

        [SerializeField]
        private GameObject levelWonUi;

        private Dictionary<GameState, GameObject> _pages;

        private InputManager _inputManager;
        private GameManager _gameManager;

        private void Awake()
        {
            _pages = new Dictionary<GameState, GameObject>
            {
                { GameState.StartUp, landingPage },
                { GameState.LevelSelection, levelSelectionPage },
                { GameState.InGame, inGameUi },
                { GameState.PostGameFail, levelFailedUi },
                { GameState.PostGameWin, levelWonUi }
            };

            DisableAllUi();

            _inputManager = Locator.Instance.InputManager;
            _gameManager = Locator.Instance.GameManager;

            _inputManager.InputsChanged += OnInputManagerChanged;
        }

        private void Start()
        {
            ChangeUi(_gameManager.CurrentState);
            _gameManager.OnGameStateChanged += ChangeUi;
        }

        private void OnDisable()
        {
            _gameManager.OnGameStateChanged -= ChangeUi;
        }

        private void ChangeUi(GameState newGameState)
        {
            DisableAllUi();

            _pages[newGameState].SetActive(true);
        }

        private void DisableAllUi()
        {
            foreach (var page in _pages.Values) page.SetActive(false);
        }

        private void OnInputManagerChanged(Inputs.Inputs inputs)
        {
            switch (_gameManager.CurrentState)
            {
                case GameState.LevelSelection:
                    LevelSelectionInputs(inputs);
                    break;
                case GameState.PostGameFail or GameState.PostGameWin:
                    ResetGame();
                    break;
            }
        }

        private void LevelSelectionInputs(Inputs.Inputs inputs)
        {
            var levelSelection = _pages[GameState.LevelSelection].GetComponent<LevelSelectionPage>();
            _gameManager.LoadLevel(levelSelection.currentLevel);
            // TODO: navigate with left/right tug and confirm with hanging
        }

        private void ResetGame()
        {
            _gameManager.ResetGame();
        }
    }
}