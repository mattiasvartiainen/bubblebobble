namespace Assets.Tests
{
    using System.Collections;
    using Assets.Scripts;
    using NUnit.Framework;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.TestTools;

    public class BubbleTest
    {
        private const int NrOfWayPoints = 5;
        private const int WayPointDistance = 20;

        private Bubble _asset;
        private LevelManager _levelManager;

        [SetUp]
        public void Setup()
        {
            _levelManager = new GameObject().AddComponent<LevelManager>();

            var so = new SerializedObject(_levelManager);
            var wp = so.FindProperty("waypoints");

            wp.arraySize = 5;
            for (var i = 0; i < NrOfWayPoints; i++)
            {
                var objectReferenceValue = new GameObject().transform;
                objectReferenceValue.position = new Vector3(i * WayPointDistance, 0, 0);
                wp.GetArrayElementAtIndex(i).objectReferenceValue = objectReferenceValue;
            }

            so.ApplyModifiedProperties();

            _asset = AssetDatabase.LoadAssetAtPath<Bubble>("Assets/Prefabs/Bubble.prefab");
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(_levelManager);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void BubbleTestSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        [UnityTest]
        public IEnumerator WhenFacingRight_ThenBubbleMovesRight()
        {
            // Arrange
            var bubble = Object.Instantiate(_asset, new Vector3(0, 0, 0), Quaternion.identity);

            var initialXPos = bubble.transform.position.x;
            bubble.transform.localScale = new Vector3(1, 1, 1);
            bubble.transform.localRotation = Quaternion.Euler(0, 0, 0);

            // Act
            yield return new WaitForSeconds(0.1f);

            // Assert
            Assert.Greater(bubble.transform.position.x, initialXPos);

            Object.Destroy(bubble);
        }
        
        [UnityTest]
        public IEnumerator WhenFacingLeft_ThenBubbleMovesLeft()
        {
            // Arrange
            var bubble = Object.Instantiate(_asset, new Vector3(0, 0, 0), Quaternion.identity);

            var initialXPos = bubble.transform.position.x;
            bubble.transform.localScale = new Vector3(-1, 1, 1);
            bubble.transform.localRotation = Quaternion.Euler(0, 180, 0);

            // Act
            yield return new WaitForSeconds(0.1f);

            // Assert
            Assert.Less(bubble.transform.position.x, initialXPos);

            Object.Destroy(bubble);
        }

        [UnityTest]
        public IEnumerator BubbleMovesToFirstWayPoint()
        {
            // Arrange
            var bubble = Object.Instantiate(_asset, new Vector3(10, 0, 0), Quaternion.identity);

            // Start
            bubble.transform.localScale = new Vector3(-1, 1, 1);
            bubble.transform.localRotation = Quaternion.Euler(0, 180, 0);

            // Move active bubble
            yield return new WaitForSeconds(0.1f);

            bubble.transform.position = new Vector3(4, 0, 0);

            // Bubble should be inactive, moved to far
            yield return new WaitForSeconds(0.1f);

            Assert.IsFalse(bubble.IsActive);

            var initialXPos = bubble.transform.position.x;

            // Move towards first WayPoint
            yield return new WaitForSeconds(0.1f);
            Assert.Less(bubble.transform.position.x, initialXPos);

            Object.Destroy(bubble);
        }
    }
}
