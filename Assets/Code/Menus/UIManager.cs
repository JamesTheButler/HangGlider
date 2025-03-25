using System.Collections.Generic;
using UnityEngine;

namespace Code.Menus
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI Pages")]
        [SerializeField]
        private Page landingPage;

        [SerializeField]
        private Page levelSelectionPage;

        [SerializeField]
        private Page inGameUi;

        [SerializeField]
        private Page levelWonUi;

        [SerializeField]
        private Page levelFailedUi;

        private Dictionary<GameState, Page> _pages;

        private GameManager _gameManager;


        private void Awake()
        {
            _pages = new Dictionary<GameState, Page>
            {
                { GameState.StartUp, landingPage },
                { GameState.LevelSelection, levelSelectionPage },
                { GameState.InGame, inGameUi },
                { GameState.PostGameFail, levelFailedUi },
                { GameState.PostGameWin, levelWonUi }
            };

            DisableAllUi();

            _gameManager = Locator.Instance.GameManager;
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

            _pages[newGameState].Open();
        }

        private void DisableAllUi()
        {
            foreach (var page in _pages.Values)
                page.Close();
        }
    }
}