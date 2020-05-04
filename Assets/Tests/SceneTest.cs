namespace Assets.Tests
{
    using System.Collections;
    using NUnit.Framework;
    using UnityEngine.SceneManagement;
    using UnityEngine.TestTools;

    public class SceneTest
    {
        [UnityTest]
        public IEnumerator TestSceneLoading()
        {
            // Store test scene
            var testScene = SceneManager.GetActiveScene();

            // Load game scene
            yield return SceneManager.LoadSceneAsync("Level02", LoadSceneMode.Additive);

            // Set the scene as active
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level02"));

            // Assert that the scene has been set as active
            Assert.IsTrue(SceneManager.GetActiveScene().name == "Level02");

            // Set the active scene back to the test scene to close the test
            SceneManager.SetActiveScene(testScene);

            // Clean up game scene
            yield return SceneManager.UnloadSceneAsync("Level02");
        }
    }
}
