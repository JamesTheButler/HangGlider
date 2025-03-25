using System.Collections.Generic;
using Core.Management;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [FormerlySerializedAs("landingPageBase")]
        [Header("UI Pages")]
        [SerializeField]
        private PageBase landingPage;

        [FormerlySerializedAs("levelSelectionPageBase")]
        [SerializeField]
        private PageBase levelSelectionPage;

        [SerializeField]
        private PageBase inGameUi;

        [SerializeField]
        private PageBase levelWonUi;

        [SerializeField]
        private PageBase levelFailedUi;

        private Dictionary<GameState, PageBase> _pages;

        private GameManager _gameManager;


        private void Awake()
        {
            _pages = new Dictionary<GameState, PageBase>
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