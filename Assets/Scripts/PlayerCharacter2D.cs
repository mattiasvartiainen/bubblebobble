using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerCharacter2D : Character
    {
        [SerializeField] private LayerMask _whatIsGround;                  // A mask determining what is ground to the character

        const float GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded

        private void Awake()
        {
            // Setting up references.
            _anim = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();
        }

        private void FixedUpdate()
        {
            _grounded = GetGrounded();
            _anim.SetBool("ground", _grounded);

            // Set the vertical animation
            _anim.SetFloat("Speed", _rigidbody2D.velocity.y);
        }
    }
}

