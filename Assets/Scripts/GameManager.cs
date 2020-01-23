using UnityEngine;

namespace Assets.Scripts
{
    using System.Diagnostics.Eventing.Reader;
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
            if (!enemies.Any())
            {
                SceneManager.LoadScene("Level02");
            }
        }
    }
}
