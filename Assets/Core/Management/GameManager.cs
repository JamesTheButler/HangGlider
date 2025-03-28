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
            Destroy(FindPlayer());

            // Remove Level
            SceneManager.UnloadSceneAsync(_currentLevel);
            _currentLevel = null;

            ChangeState(GameState.StartUp);
        }

        public void CollisionWithObstacle()
        {
            FindPlayer()?.SetEnabled(false);
            ChangeState(GameState.PostGameFail);
        }

        public void CollisionWithGoal()
        {
            FindPlayer()?.SetEnabled(false);
            ChangeState(GameState.PostGameWin);
        }

        public void PlayerLetGo()
        {
            FindPlayer()?.SetEnabled(false);
            ChangeState(GameState.PostGameFail);
        }

        [CanBeNull]
        private static GliderController FindPlayer()
        {
            return GameObject.FindGameObjectWithTag(Tags.Player)?.GetComponent<GliderController>();
        }
    }
}