namespace Assets.Scripts
{
    using UnityEngine;

    [RequireComponent(typeof(Controller2D))]
    public class Character2 : MonoBehaviour
    {
        public float JumpHeight = 3;
        public float TimeToJumpApex = .4f;
        public float MoveSpeed = 4;

        protected Animator Anim;

        readonly float accelerationTimeAirborne = .2f;
        readonly float accelerationTimeGrounded = .1f;

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
            if (transform.position.y < -10f)
            {
                transform.position = new Vector3(transform.position.x, 7f, transform.position.z); 
            }

            if (_controller.Collisions.Above || _controller.Collisions.Below)
            {
                _velocity.y = 0;
            }

            var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (Input.GetKeyDown(KeyCode.Space) && _controller.Collisions.Below)
            {
                _velocity.y = _jumpVelocity;
            }

            Anim.SetBool("Fire", Input.GetButtonDown("Fire1"));

            Anim.SetBool("ground", _controller.Collisions.Below);

            var targetVelocityX = input.x * MoveSpeed;
            _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing,
                (_controller.Collisions.Below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            _velocity.y += _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);

            var speed = Mathf.Abs(_velocity.x);
            if (!_controller.Collisions.Below)
            {
                speed = 0;
            }

            Anim.SetBool("IsWalking", speed > 0.01f && _controller.Collisions.Below);

            Anim.SetFloat("Speed", speed);
        }

        void OnCollisionEnter2D(Collision target)
        {
            Debug.Log($"OnCollisionEnter {target.gameObject.name}");

            if (target.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Bubblun is dead!");
            }
        }
    }
}
