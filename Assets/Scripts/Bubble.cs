namespace Assets.Scripts
{
    using System;
    using UnityEngine;

    public class Bubble : MonoBehaviour
    {
        public float speed = 20f;
        public Rigidbody2D rb;
        public bool IsActive = true;

        private Vector3 startPosition;

        private void Awake()
        {
            startPosition = transform.position;
        }

        // Start is called before the first frame update
        private void Start()
        {
            rb.velocity = transform.right * speed;
        }

        private void Update()
        {
            //Physics2D.IgnoreCollision(GameObject.FindWithTag("Enemy").GetComponent<BoxCollider2D>(),
            //    GetComponent<CircleCollider2D>(),
            //    true);

            if (Math.Abs(transform.position.x - startPosition.x) > 5)
            {
                //Destroy(gameObject);
                rb.velocity = transform.up * 2f;
                IsActive = false;
                gameObject.layer = LayerMask.NameToLayer("PickupObjects");

            }
        }

        private void OnTriggerEnter2D(Collider2D target)
        {
            Debug.Log($"Bubble OnTriggerEnter2D {target.name}");

            if (target.name.Equals("Border") && Math.Abs(rb.velocity.x) > 0.01f)
            {
                rb.velocity = new Vector2(0f, rb.velocity.y);
                rb.velocity = transform.up * 2f;
            }

            if (IsActive)
            {
                var enemy = target.GetComponent<Enemy2>();
                if (enemy != null && !enemy.gameObject.tag.Equals("BubbledEnemy"))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}