using System;
using Core.Utility;
using Features.Player;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Management
{
    public class GameManager : MonoBehaviour
    {
        private string _currentLevel;

        public GameState CurrentState { get; private set; }

        private void Start()
        {
            ChangeState(GameState.StartUp);
        }

        public event Action<GameState> OnGameStateChanged;

        private void ChangeState(GameState newState)
        {
            if (CurrentState == newState)
                return;

            CurrentState = newState;
            OnGameStateChanged?.Invoke(newState);
        }

        public void StartGame()
        {
            ChangeState(GameState.LevelSelection);
        }

        public void LoadLevel(string levelName)
        {
            ChangeState(GameState.InGame);

            if (!string.IsNullOrEmpty(_currentLevel))
                SceneManager.UnloadSceneAsync(_currentLevel);

            _currentLevel = levelName;
            SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        }

        public void ResetGame()
        {
            // despawn player
            Destroy(Finder.Player);

            // Remove Level
            SceneManager.UnloadSceneAsync(_currentLevel);
            _currentLevel = null;

            ChangeState(GameState.StartUp);
        }

        public void CollisionWithObstacle()
        {
            Finder.PlayerController?.SetEnabled(false);
            ChangeState(GameState.PostGameFail);
        }

        public void CollisionWithGoal()
        {
            Finder.PlayerController?.SetEnabled(false);
            ChangeState(GameState.PostGameWin);
        }

        public void PlayerLetGo()
        {
            Finder.PlayerController?.SetEnabled(false);
            ChangeState(GameState.PostGameFail);
        }
    }
}