namespace Assets.Tests.Services
{
    using Assets.Scripts.Services;
    using Assets.Scripts.Wrappers;
    using FakeItEasy;
    using NUnit.Framework;
    using UnityEngine;

    public class BubbleMovementCalculatorTest
    {
        private ITransform _transform;
        private Transform[] _wayPoints;

        [SetUp]
        public void BaseSetUp()
        {
            _transform = A.Fake<ITransform>();
            A.CallTo(() => _transform.LocalScale).Returns(new Vector3(1.0f, 0f, 0f));

            _wayPoints = new Transform[3];

            _wayPoints[0] = new GameObject().transform;
            _wayPoints[0].position = new Vector3(10, 0, 0);

            _wayPoints[1] = new GameObject().transform;
            _wayPoints[1].position = new Vector3(20, 0, 0);

            _wayPoints[2] = new GameObject().transform;
            _wayPoints[2].position = new Vector3(20, -10, 0);
        }

        [TestCase(1f, 1f, -1f)]
        [TestCase(2f, 1f, -2f)]
        [TestCase(5f, 2f, -10f)]
        public void WhenBubbleIsActive_ThenTheSpeedIsCorrect(float speed, float deltaTime, float expected)
        {
            // Arrange
            var calculator = new BubbleMovementCalculator(_transform, new Transform[] {}, speed);

            // Act
            var result = calculator.GetActiveMovementDirection(deltaTime);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void WhenBubbleIsInactive_ThenBubbleMovesTowardsFirstWayPoint()
        {
            // Arrange
            A.CallTo(() => _transform.Position).Returns(Vector3.zero);

            var calculator = new BubbleMovementCalculator(_transform, _wayPoints);

            var initialDistance = Vector3.Distance(_wayPoints[0].position, _transform.Position);

            // Act
            var result = calculator.GetInactiveMovementPosition(1f);

            // Assert
            var newDistance = Vector3.Distance(_wayPoints[0].position, new Vector3(result.x, result.y, 0f));
            Assert.Less(newDistance, initialDistance);
        }

        [Test]
        public void WhenBubbleIsInactiveAndFirstWayPointIsVisited_ThenBubbleMovesTowardsSecondWayPoint()
        {
            // Arrange
            A.CallTo(() => _transform.Position).Returns(new Vector3(10, 0, 0));

            var calculator = new BubbleMovementCalculator(_transform, _wayPoints);

            var initialDistance = Vector3.Distance(_wayPoints[1].position, _transform.Position);

            // Act
            var result = calculator.GetInactiveMovementPosition(1f);

            // Assert
            var newDistance = Vector3.Distance(_wayPoints[1].position, new Vector3(result.x, result.y, 0f));
            Assert.Less(newDistance, initialDistance);
        }

        [Test]
        public void WhenBubbleIsInactiveAndClosestToSecondWayPoint_ThenBubbleMovesTowardsSecondWayPoint()
        {
            // Arrange
            A.CallTo(() => _transform.Position).Returns(new Vector3(16, 0, 0));

            var calculator = new BubbleMovementCalculator(_transform, _wayPoints);

            var initialDistance = Vector3.Distance(_wayPoints[1].position, _transform.Position);

            // Act
            var result = calculator.GetInactiveMovementPosition(1f);

            // Assert
            var newDistance = Vector3.Distance(_wayPoints[1].position, new Vector3(result.x, result.y, 0f));
            Assert.Less(newDistance, initialDistance);
        }

        [Test]
        public void WhenBubbleIsInactive_ThenBubbleShouldNot()
        {
            // Arrange
            A.CallTo(() => _transform.Position).Returns(new Vector3(16, 0, 0));

            var calculator = new BubbleMovementCalculator(_transform, _wayPoints);

            var initialDistance = Vector3.Distance(_wayPoints[1].position, _transform.Position);

            // Act
            var result = calculator.GetInactiveMovementPosition(1f);

            // Assert
            var newDistance = Vector3.Distance(_wayPoints[1].position, new Vector3(result.x, result.y, 0f));
            Assert.Less(newDistance, initialDistance);
        }
    }
}
