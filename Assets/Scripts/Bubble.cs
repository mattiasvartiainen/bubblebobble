namespace Assets.Scripts
{
    using System;
    using UnityEngine;

    public class Bubble : MonoBehaviour
    {
        public float speed = 10f;
        public bool IsActive = true;

        private Vector3 startPosition;

        private Controller2D _controller;

        private LevelManager _levelManager;

        private Transform[] _wayPoints;
        private int waypointIndex = 0;

        private Vector3 activeDestination;
        private Vector3 _velocity;

        private void Awake()
        {
            startPosition = transform.position;
        }

        private void Start()
        {
            _controller = GetComponent<Controller2D>();
            _levelManager = FindObjectOfType<LevelManager>();
            _wayPoints = _levelManager.GetWaypoints();

            _controller.OnChangeDirection += ChangeDirection;

            var direction = transform.right.x > 0f ? 5f : -5f;
            _velocity = new Vector3(direction, 0f, 0f);

            var destX = 20 * transform.right.x;
            activeDestination = new Vector3( transform.position.x + direction, transform.position.y, 0f);
            Debug.Log($"Bubble destination {activeDestination.x}");
        }

        private void ChangeDirection(bool facingRight)
        {
            IsActive = false;
        }

        private void Update()
        {
            if (IsActive)
            {
                DoActiveMovement();
            }
            else
            {
                DoInactiveMovement();
            }
        }

        private void DoInactiveMovement()
        {
            gameObject.layer = LayerMask.NameToLayer("PickupObjects");

            Move();
        }

        private void Move()
        {
            var bubble = transform.position;
            var wayPoint = _wayPoints[waypointIndex].transform.position;

            if (waypointIndex > _wayPoints.Length - 1) return;

            var dist = Vector3.Distance(wayPoint, bubble);
            for(var i=waypointIndex; i<_wayPoints.Length; i++)
            {
                if (i == waypointIndex) continue;

                var w = _wayPoints[i].transform.position;
                var abs = Math.Abs(w.y - wayPoint.y);
                if (abs < 0.5f)
                {
                    var newDist = Vector3.Distance(w, bubble);
                    if (newDist < dist)
                    {
                        waypointIndex = i;
                        wayPoint = _wayPoints[waypointIndex].transform.position;
                    }
                }
            }

            var f = Math.Abs(bubble.y - wayPoint.y);
            if (f < 1.0f)
            {
                if (Math.Abs(bubble.x - wayPoint.x) < 0.5f)
                {
                    waypointIndex += 1;
                    wayPoint = _wayPoints[waypointIndex].transform.position;
                }
            }

            var destination = new Vector3(bubble.x, wayPoint.y, 0);

            if (Vector3.Distance(bubble, destination) < 0.5f)
            {
                destination = new Vector3(wayPoint.x, bubble.y, 0);
            }

            var maxDistanceDelta = (speed / 8) * Time.deltaTime;
            transform.position = Vector2.MoveTowards(bubble,
                destination,
                maxDistanceDelta);
        }

        private void DoActiveMovement()
        {
            if (_controller.Collisions.Above || _controller.Collisions.Below)
            {
            }

            if (_controller.Collisions.Left || _controller.Collisions.Right)
            {
                IsActive = false;
            }

            if (Math.Abs(transform.position.x - startPosition.x) > 5)
            {
                IsActive = false;
            }

            var maxDistanceDelta = speed / 4 * Time.deltaTime;
            var pos = Vector2.MoveTowards(transform.position,
                activeDestination,
                maxDistanceDelta);

            var to = new Vector3(transform.position.x - pos.x, 0, 0);

            var velocity = (new Vector2(transform.position.x, transform.position.y) - pos);

            var dir = transform.localScale.x > 0 ? -8 : 8;
            _controller.Move(new Vector3(dir * Time.deltaTime, 0, 0));
        }

        void OnCollisionEnter2D(Collision target)
        {
            //Debug.Log($"OnCollisionEnter Bubble {target.gameObject.name}");
        }

        private void OnTriggerEnter2D(Collider2D target)
        {
            Debug.Log($"Bubble OnTriggerEnter2D {target.name}");

            if (target.name.Equals("Border") )
            {
                IsActive = false;
            }

            if (IsActive)
            {
                var enemy = target.GetComponent<Enemy2>();
                if (enemy != null && !enemy.gameObject.tag.Equals("BubbledEnemy"))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}