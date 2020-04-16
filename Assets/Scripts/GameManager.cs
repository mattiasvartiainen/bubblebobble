using UnityEngine;

namespace Assets.Scripts
{
    using System.Collections;
    using System.Collections.Generic;
    //using System.Diagnostics.Eventing.Reader;
    using System.Linq;
    using UnityEngine.SceneManagement;

    public enum GameState { INTRO, MAIN_MENU }

    public delegate void OnStateChangeHandler();

    public class GameManager : MonoBehaviour
    {
        public int score;

        protected GameManager() { }

        public static GameManager instance;
        public event OnStateChangeHandler OnStateChange;

        public GameState gameState { get; private set; }

        void Awake()
        {
            MakeSingleton();
        }

        private void MakeSingleton()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void SetGameState(GameState state)
        {
            this.gameState = state;
            OnStateChange();
        }

        public void OnApplicationQuit()
        {
            GameManager.instance = null;
        }

        public void EnemyKilled()
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            var bubbledEnemies = GameObject.FindGameObjectsWithTag("BubbledEnemy");
            var enemiesExists = enemies.Any() || bubbledEnemies.Length > 1;
            if (!enemiesExists)
            {
                StartCoroutine(LevelComplete());
            }
        }

        IEnumerator LevelComplete()
        {
            yield return new WaitForSeconds(5);

            var nextScene = SceneManager.GetActiveScene().name.Equals("Level02") ? "Level05" : "Level02";
            SceneManager.LoadScene(nextScene);
        }
    }
}
