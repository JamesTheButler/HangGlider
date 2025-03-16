using System;
using System.Collections.Generic;
using Code;
using Code.Inputs;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private KeyboardFlightInputs keyboardFlightInputs;

    [Header("Pages")]
    [SerializeField]
    private GameObject landingPage;

    [SerializeField]
    private GameObject levelSelectionPage;

    [SerializeField]
    private GameObject inGameUi;

    private Dictionary<UiState, GameObject> _pages;
    private UiState _uiState;

    private UiState UiState
    {
        get => _uiState;
        set => ChangeUiState(value);
    }

    private void Start()
    {
        _pages = new Dictionary<UiState, GameObject>
        {
            { UiState.LandingPage, landingPage },
            { UiState.LevelSelectionPage, levelSelectionPage },
            { UiState.InGame, inGameUi }
        };

        keyboardFlightInputs.InputsChanged += OnInputsChanged;

        UiState = UiState.LandingPage;
    }

    private void OnInputsChanged(Inputs inputs)
    {
        switch (UiState)
        {
            case UiState.LandingPage:
                StartGame();
                break;
            case UiState.LevelSelectionPage:
                break;
            case UiState.InGame:
                // ignore, this is handled by the player
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ChangeUiState(UiState newPage)
    {
        foreach (var page in _pages.Values) page.SetActive(false);

        _pages[newPage].SetActive(true);
    }

    private void StartGame()
    {
        UiState = UiState.InGame;

        var player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<GliderController>().enabled = true;
    }
}