using System;
using System.Collections.Generic;
using Code;
using UnityEngine;

public enum UiState
{
    LandingPage,
    LevelSelectionPage,
    InLevel
}

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private FlightInputs flightInputs;

    [Header("Pages")]
    [SerializeField]
    private GameObject landingPage;

    [SerializeField]
    private GameObject levelSelectionPage;

    [SerializeField]
    private GameObject inGameUi;

    private Dictionary<UiState, GameObject> _pages;

    private UiState UiState { get; set; }

    private void Start()
    {
        _pages = new Dictionary<UiState, GameObject>
        {
            { UiState.LandingPage, landingPage },
            { UiState.LevelSelectionPage, levelSelectionPage },
            { UiState.InLevel, inGameUi }
        };

        flightInputs.InputsChanged += OnInputsChanged;

        UiState = UiState.LandingPage;
    }

    private void OnInputsChanged(Inputs inputs)
    {
        switch (UiState)
        {
            case UiState.LandingPage:
                landingPage.SetActive(false);
                levelSelectionPage.SetActive(true);
                break;
            case UiState.LevelSelectionPage:
                Debug.LogWarning("Level Selection Input Handling goes here.");
                break;
            case UiState.InLevel:
                // ignore, this is handled by the player
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ChangeUiState(UiState newPage)
    {
        foreach (var page in _pages.Values) page.SetActive(false);

        UiState = newPage;

        _pages[newPage].SetActive(true);
    }
}