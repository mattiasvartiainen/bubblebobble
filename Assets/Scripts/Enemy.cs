using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    using UnityEngine;

    public class Enemy : Character
    {
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

            Move(maxSpeed, false);
        }
    }
}