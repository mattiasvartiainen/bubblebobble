using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private float _jumpForce = 400f;                  // Amount of force added when the player jumps.
        [SerializeField] protected float maxSpeed = 10f;    // The fastest the character can travel in the x axis.
        [SerializeField] protected bool _airControl = false;                 // Whether or not a player can steer while jumping;

        protected bool _grounded;                           // Whether or not the character is grounded.
        protected Rigidbody2D _rigidbody2D;
        protected bool _facingRight = false;                // For determining which way the player is currently facing.

        protected Animator _anim;            // Reference to the player's animator component.
        protected BoxCollider2D boxCollider;

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
                _anim.SetBool("ground", false);
                _rigidbody2D.AddForce(new Vector2(0f, _jumpForce));
            }
        }

        protected bool GetGrounded()
        {
            var grounded = false;

            var hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

            foreach (var hit in hits)
            {
                // Ignore our own collider.
                if (hit == boxCollider)
                    continue;

                var colliderDistance = hit.Distance(boxCollider);

                // Ensure that we are still overlapping this collider.
                // The overlap may no longer exist due to another intersected collider
                // pushing us out of this one.
                //if (colliderDistance.isOverlapped)
                {
                    //transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                    // If we intersect an object beneath us, set grounded to true. 
                    if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90) // && velocity.y < 0)
                    {
                        grounded = true;
                    }
                }
            }

            return grounded;
        }

        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            _facingRight = !_facingRight;

            // Multiply the player's x local scale by -1.
            //Vector3 theScale = transform.localScale;
            //theScale.x *= -1;
            //transform.localScale = theScale;
            transform.Rotate(0, 180f, 0);
        }
    }
}
