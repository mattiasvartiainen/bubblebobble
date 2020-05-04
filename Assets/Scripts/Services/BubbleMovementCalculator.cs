namespace Assets.Scripts.Services
{
    using System;
    using Assets.Scripts.Wrappers;
    using UnityEngine;

    public class BubbleMovementCalculator : IBubbleMovementCalculator
    {
        private readonly float _activeSpeed;
        private readonly float _inactiveSpeed;
        private float _inactiveOffset = 0;
        private readonly ITransform _transform;
        private readonly Transform[] _wayPoints;
        private int _wayPointIndex;
        private int MaxWayPointIndex => _wayPoints.Length - 1;

        public BubbleMovementCalculator(ITransform transform, Transform[] wayPoints, float activeSpeed = 8f, float inactiveSpeed = 2.5f)
        {
            _transform = transform;
            _wayPoints = wayPoints;
            _activeSpeed = activeSpeed;
            _inactiveSpeed = inactiveSpeed;
        }

        public float GetActiveMovementDirection(float deltaTime)
        {
            var dir = _transform.LocalScale.x > 0 ? -_activeSpeed : _activeSpeed;
            return dir * deltaTime;
        }

        public Vector2 GetInactiveMovementPosition(float deltaTime)
        {
            var bubblePos = _transform.Position;
            var wayPoint = _wayPoints[_wayPointIndex].transform.position;

            if (_wayPointIndex > _wayPoints.Length - 1) return Vector2.zero;

            wayPoint = SetClosestWayPoint(wayPoint, bubblePos);

            wayPoint = SetNewWayPointOnCollision(bubblePos, wayPoint, deltaTime);

            var destination = new Vector3(bubblePos.x, wayPoint.y, 0);
            
            if (Vector3.Distance(bubblePos, destination) < 0.5f)
            {
                destination = new Vector3(wayPoint.x, bubblePos.y, 0);
            }

            destination.y += (float)Math.Sin(_inactiveOffset);

            var maxDistanceDelta = _inactiveSpeed * deltaTime;
            return Vector2.MoveTowards(bubblePos, destination, maxDistanceDelta);
        }

        private Vector3 SetNewWayPointOnCollision(Vector3 bubblePos, Vector3 wayPoint, float deltaTime)
        {
            var yDist = Vector2.Distance(new Vector2(0, bubblePos.y), new Vector2(0, wayPoint.y));
            if (yDist < 1.0f)
            {
                var xDist = Vector2.Distance(new Vector2(0, bubblePos.x), new Vector2(0, wayPoint.x));
                if (xDist < 0.5f)
                {
                    if (_wayPointIndex < MaxWayPointIndex)
                    {
                        _wayPointIndex += 1;
                    }
                    else
                    {
                        _inactiveOffset += deltaTime * 10f;
                    }

                    wayPoint = _wayPoints[_wayPointIndex].transform.position;
                }
            }

            return wayPoint;
        }

        private Vector3 SetClosestWayPoint(Vector3 wayPoint, Vector3 bubblePos)
        {
            var dist = Vector3.Distance(wayPoint, bubblePos);
            for (var i = _wayPointIndex; i <= MaxWayPointIndex; i++)
            {
                if (i == _wayPointIndex) continue;

                var w = _wayPoints[i].transform.position;
                var abs = Math.Abs(w.y - wayPoint.y);
                
                if (!(abs < 0.5f)) continue;

                var newDist = Vector3.Distance(w, bubblePos);
                
                if (!(newDist < dist)) continue;

                _wayPointIndex = i;
                wayPoint = _wayPoints[_wayPointIndex].transform.position;
            }

            return wayPoint;
        }
    }

    public interface IBubbleMovementCalculator
    {
        float GetActiveMovementDirection(float deltaTime);

        Vector2 GetInactiveMovementPosition(float deltaTime);
    }
}
