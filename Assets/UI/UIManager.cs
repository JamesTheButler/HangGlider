using System.Collections.Generic;
using Core.Management;
using NaughtyAttributes;
using UI.Pages;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField, Required]
        private GameManager gameManager;

        [Header("UI Pages"), SerializeField]
        private PageBase landingPage;

        [SerializeField]
        private PageBase levelSelectionPage;

        [SerializeField]
        private PageBase inGameUi;

        [SerializeField]
        private PageBase levelWonUi;

        [SerializeField]
        private PageBase levelFailedUi;

        private Dictionary<GameState, PageBase> _pages;


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
        }

        private void Start()
        {
            ChangeUi(gameManager.CurrentState);
            gameManager.OnGameStateChanged += ChangeUi;
        }

        private void OnDisable()
        {
            gameManager.OnGameStateChanged -= ChangeUi;
        }

        private void ChangeUi(GameState newGameState)
        {
            DisableAllUi();

            _pages[newGameState].Open();
        }

        private void DisableAllUi()
        {
            foreach (var page in _pages.Values)
            {
                page.Close();
            }
        }
    }
}