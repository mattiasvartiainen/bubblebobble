using UnityEngine;

namespace Assets.Scripts
{
    public class DeathEffect : MonoBehaviour
    {
        public float JumpHeight = 8;
        public float TimeToJumpApex = .8f;

        readonly float accelerationTimeAirborne = .4f;
        readonly float accelerationTimeGrounded = .3f;
        readonly float moveSpeed = 6;

        private float _gravity;
        private float _jumpVelocity;
        private Vector3 _velocity;
        private float _velocityXSmoothing;

        private Controller2D _controller;

        public GameObject[] pickupObjects;

        // Start is called before the first frame update
        void Start()
        {
            _controller = GetComponent<Controller2D>();

            _gravity = -(2 * JumpHeight) / Mathf.Pow(TimeToJumpApex, 2);
            _jumpVelocity = Mathf.Abs(_gravity) * TimeToJumpApex;
            _velocity.y = _jumpVelocity;
        }

        // Update is called once per frame
        void Update()
        {
            if (_controller.Collisions.Below)
            {
                _velocity.y = 0;
                _velocity.x = 0;

                var pickupObject = pickupObjects[Random.Range(0, pickupObjects.Length)];
                Instantiate(pickupObject, transform.position, Quaternion.identity);

                Destroy(gameObject);

                return;
            }

            var targetVelocityX = moveSpeed;
            _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing,
                (_controller.Collisions.Below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            _velocity.y += _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);

            var speed = Mathf.Abs(_velocity.x);
            if (!_controller.Collisions.Below)
            {
                speed = 0;
            }
        }
    }
}
