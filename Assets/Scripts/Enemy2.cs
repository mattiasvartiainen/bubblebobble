namespace Assets.Scripts
{
    using UnityEngine;

    [RequireComponent(typeof(Controller2D))]
    public class Enemy2 : MonoBehaviour
    {
        public float JumpHeight = 4;
        public float TimeToJumpApex = .4f;

        protected Animator Anim;

        readonly float accelerationTimeAirborne = .2f;
        readonly float accelerationTimeGrounded = .1f;
        readonly float moveSpeed = 3;

        private float _gravity;
        private float _jumpVelocity;
        private Vector3 _velocity;
        private float _velocityXSmoothing;

        private Controller2D _controller;

        void Start()
        {
            _controller = GetComponent<Controller2D>();
            Anim = GetComponent<Animator>();

            _gravity = -(2 * JumpHeight) / Mathf.Pow(TimeToJumpApex, 2);
            _jumpVelocity = Mathf.Abs(_gravity) * TimeToJumpApex;
            print("Gravity: " + _gravity + "  Jump Velocity: " + _jumpVelocity);
        }

        void Update()
        {
            if (_controller.Collisions.Above || _controller.Collisions.Below)
            {
                _velocity.y = 0;
            }

            if (_controller.Collisions.Left || _controller.Collisions.Right)
            {
                _controller.Flip();
            }

            var input = new Vector2(_controller.FacingRight ? -1.0f : 1.0f, 0.0f);

            Anim.SetBool("ground", _controller.Collisions.Below);

            var targetVelocityX = input.x * moveSpeed;
            _velocity.x = targetVelocityX; // Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing, (_controller.Collisions.Below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            _velocity.y += _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);

            var speed = Mathf.Abs(_velocity.x);
            if (!_controller.Collisions.Below)
            {
                speed = 0;
            }
            Anim.SetFloat("Speed", speed);
        }
    }
}
