namespace Assets.Scripts
{
    using System;
    using Assets.Scripts.Services;
    using Assets.Scripts.Wrappers;
    using UnityEngine;

    [RequireComponent(typeof(Controller2D))]
    [RequireComponent(typeof(LevelManager))]
    public class Bubble : MonoBehaviour
    {
        public bool IsActive = true;

        private Vector3 _startPosition;

        private Controller2D _controller;

        private LevelManager _levelManager;

        private IBubbleMovementCalculator _bubbleMovementCalculator;

        private void Awake()
        {
            _startPosition = transform.position;
        }

        private void Start()
        {
            _controller = GetComponent<Controller2D>();
            _levelManager = FindObjectOfType<LevelManager>();

            _controller.OnChangeDirection += ChangeDirection;

            _bubbleMovementCalculator = new BubbleMovementCalculator(transform.Wrap(), _levelManager.GetWaypoints());
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

            var position = _bubbleMovementCalculator.GetInactiveMovementPosition(Time.deltaTime);
            transform.position = position;
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

            if (Math.Abs(transform.position.x - _startPosition.x) > 5)
            {
                IsActive = false;
            }

            var dir = _bubbleMovementCalculator.GetActiveMovementDirection(Time.deltaTime);
            _controller.Move(new Vector3(dir, 0, 0));
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
                    Debug.Log($"Bubble hit enemy in OnTriggerEnter2D {target.name}");
                    Destroy(gameObject);
                }
            }
        }
    }
}