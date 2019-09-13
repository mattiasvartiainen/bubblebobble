namespace Assets.Scripts
{
    using UnityEngine;

    [RequireComponent(typeof(Controller2D))]
    public class Character2 : MonoBehaviour
    {
        public float JumpHeight = 4;
        public float timeToJumpApex = .4f;
        float accelerationTimeAirborne = .2f;
        float accelerationTimeGrounded = .1f;
        float moveSpeed = 6;

        float gravity;
        float jumpVelocity;
        Vector3 velocity;
        float velocityXSmoothing;

        Controller2D controller;

        protected Animator _anim;

        void Start()
        {
            controller = GetComponent<Controller2D>();
            _anim = GetComponent<Animator>();

            gravity = -(2 * JumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
            print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
        }

        void Update()
        {
            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }

            var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
            {
                velocity.y = jumpVelocity;
            }

            _anim.SetBool("ground", controller.collisions.below);

            var targetVelocityX = input.x * moveSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            var speed = Mathf.Abs(velocity.x);
            if (!controller.collisions.below)
            {
                speed = 0;
            }
            _anim.SetFloat("Speed", speed);
        }
    }
}
