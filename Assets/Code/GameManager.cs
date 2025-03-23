using System;
using Code.Menus;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code
{
    public class GameManager : MonoBehaviour
    {
        private string _currentScene;

        public GameState CurrentState { get; private set; }

        private void Start()
        {
            ChangeState(GameState.StartUp);
        }

        public event Action<GameState> OnGameStateChanged;

        private void ChangeState(GameState newState)
        {
            if (CurrentState == newState)
            {
                return;
            }

            CurrentState = newState;
            OnGameStateChanged?.Invoke(newState);
        }

        public void LoadLevel(string levelName)
        {
            Debug.LogWarning("Level Selection not implemented yet");
            ChangeState(GameState.InGame);

            SceneManager.UnloadSceneAsync(levelName);

            _currentScene = levelName;
            SceneManager.LoadScene(levelName, LoadSceneMode.Additive);


            var player = FindPlayer();
            player.GetComponent<GliderController>().enabled = true;

            var uiMgr = FindAnyObjectByType<InGameUIManager>();
            uiMgr.Initialize();
        }

        public void ResetGame()
        {
            // despawn player
            Destroy(FindPlayer());

            // Remove Level
            SceneManager.UnloadSceneAsync(_currentScene);

            ChangeState(GameState.StartUp);
        }

        private static GameObject FindPlayer()
        {
            return GameObject.FindGameObjectWithTag("Player");
        }
    }
}