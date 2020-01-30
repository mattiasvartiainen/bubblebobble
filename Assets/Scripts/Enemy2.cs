namespace Assets.Scripts
{
    using System.Linq.Expressions;
    using UnityEngine;

    [RequireComponent(typeof(Controller2D))]
    public class Enemy2 : MonoBehaviour
    {
        public float Cooldown = 10;
        public float Timer;
        public GameObject deathEffectPrefab;

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

        private bool bubbled = false;

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

            if (bubbled)
            {
                AnimateBubbled();
                Timer += Time.deltaTime;
                if (Timer >= Cooldown)
                {
                    Respawn();
                }
            }

            if(!bubbled)
            {
                AnimateMovement();
            }
        }

        private void AnimateBubbled()
        {
            _velocity.y = -_gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }

        private void AnimateMovement()
        {
            if (_controller.Collisions.Left || _controller.Collisions.Right)
            {
                _controller.Flip();
            }

            var input = new Vector2(_controller.FacingRight ? -1.0f : 1.0f, 0.0f);

            Anim.SetBool("ground", _controller.Collisions.Below);

            var targetVelocityX = input.x * moveSpeed;
            _velocity.x =
                targetVelocityX; // Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing, (_controller.Collisions.Below) ? accelerationTimeGrounded : accelerationTimeAirborne);
            _velocity.y += _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);

            var speed = Mathf.Abs(_velocity.x);
            if (!_controller.Collisions.Below)
            {
                speed = 0;
            }

            Anim.SetFloat("Speed", speed);
        }

        public void HitByBubble()
        {
            if (bubbled == true)
                return;

            Timer = 0;
            bubbled = true;
            gameObject.tag = "BubbledEnemy";
            Anim.SetBool("Bubbled", bubbled);
        }

        private void Respawn()
        {
            bubbled = false;
            gameObject.tag = "Enemy";
            Anim.SetBool("Bubbled", bubbled);
        }

        void OnTriggerEnter2D(Collider2D target)
        {
            Debug.Log($"Enemy OnTriggerEnter2D {target.name}");

            var bubble = target.GetComponent<Bubble>();
            if (bubble != null && bubble.IsActive)
            {
                HitByBubble();
            }
            else if (bubble != null)
            {
                //Physics2D.IgnoreCollision(bubble.gameObject.GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>());
            }

            if (target.gameObject.tag.Equals("Player"))
            {
                if (bubbled == true)
                {
                    Die();
                }
            }
        }

        private void Die()
        {
            // TODO: Maybe use object pool for bubbles?
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
            GameManager.instance.EnemyKilled();
        }
    }
}
