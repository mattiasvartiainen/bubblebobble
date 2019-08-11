using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float maxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float _jumpForce = 400f;                  // Amount of force added when the player jumps.
        [SerializeField] private bool _airControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask _whatIsGround;                  // A mask determining what is ground to the character

        const float GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
        private bool _grounded;            // Whether or not the player is grounded.
        private Animator _anim;            // Reference to the player's animator component.
        private Rigidbody2D _rigidbody2D;
        private bool _facingRight = false;  // For determining which way the player is currently facing.

        private BoxCollider2D boxCollider;

        private void Awake()
        {
            // Setting up references.
            _anim = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();
        }


        private void FixedUpdate()
        {
            _grounded = false;

            var hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

            foreach (var hit in hits)
            {
                // Ignore our own collider.
                if (hit == boxCollider)
                    continue;

                _grounded = true;
            }
            _anim.SetBool("Ground", _grounded);

            // Set the vertical animation
            _anim.SetFloat("vSpeed", _rigidbody2D.velocity.y);
        }


        public void Move(float move, bool jump)
        {
            //only control the player if grounded or airControl is turned on
            if (_grounded || _airControl)
            {
                // The Speed animator parameter is set to the absolute value of the horizontal input.
                _anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                _rigidbody2D.velocity = new Vector2(move * maxSpeed, _rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !_facingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && _facingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (_grounded && jump) // && _anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                _grounded = false;
                _anim.SetBool("Ground", false);
                _rigidbody2D.AddForce(new Vector2(0f, _jumpForce));
            }
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            _facingRight = !_facingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}

