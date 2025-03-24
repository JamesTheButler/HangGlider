using System.Collections;
using System.Collections.Generic;
using Code.Inputs;
using UnityEngine;

namespace Code.Menus
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private float postGameResetTime;

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

        private Coroutine _postGameResetCoroutine;

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

            if (newGameState == GameState.PostGameFail || newGameState == GameState.PostGameWin)
            {
                StartResetTimer();
            }
        }

        private void StartResetTimer()
        {
            if (_postGameResetCoroutine != null)
            {
                StopCoroutine(_postGameResetCoroutine);
            }

            _postGameResetCoroutine = StartCoroutine(ResetAfterDelay(postGameResetTime));
        }

        private IEnumerator ResetAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            ResetGame();
        }

        private void DisableAllUi()
        {
            foreach (var page in _pages.Values) page.SetActive(false);
        }

        private void OnInputManagerChanged(Inputs.Inputs inputs)
        {
            switch (_gameManager.CurrentState)
            {
                case GameState.StartUp:
                    _gameManager.StartGame();
                    break;
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
            if (_postGameResetCoroutine != null)
            {
                StopCoroutine(_postGameResetCoroutine);
                _postGameResetCoroutine = null;
            }

            _gameManager.ResetGame();
        }
    }
}