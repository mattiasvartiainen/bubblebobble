namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public class Enemy : Character
    {
        public GameObject tilemapGameObject;
        Tilemap tilemap;

        private void Awake()
        {
            // Setting up references.
            _anim = GetComponent<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();

            if (tilemapGameObject != null)
            {
                tilemap = tilemapGameObject.GetComponent<Tilemap>();
            }

            //Physics2D.queriesStartInColliders = false;
        }

        private void FixedUpdate()
        {
            _grounded = GetGrounded();
            _anim.SetBool("ground", _grounded);

            //if (DoTurn())
            //{
            //    Flip();
            //}

            var speed = FacingRight ? maxSpeed : -maxSpeed;
            Move(speed, false);
        }

        bool DoTurn()
        {
            var position = transform.position;
            var direction = FacingRight ? Vector2.right : Vector2.left;
            var distance = 1.1f;

            var hit = Physics2D.Raycast(position, direction, distance);
            if (hit.collider != null)
            {
                return true;
            }

            return false;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            return;

            if (collision.gameObject.tag == "tile")
            {
                // Destroy the tile the rockets collided with
                Destroy(collision.GetComponent<Collider>().gameObject);
                // Destroy the rocket itself
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.otherCollider.gameObject.name.Equals("WallCollider") && collision.gameObject.CompareTag("Border"))
            {
                Flip();
            }

            return;

            Vector3 hitPosition = Vector3.zero;
            if (tilemap != null && tilemapGameObject == collision.gameObject)
            {
                foreach (ContactPoint2D hit in collision.contacts)
                {
                    hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                    hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                    //tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
                }
            }

            if (collision.gameObject.CompareTag("border"))
            {
                Flip();
            }
            if (collision.collider.CompareTag("border"))
            {
                Flip();
            }
        }
    }
}