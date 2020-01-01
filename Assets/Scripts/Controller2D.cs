namespace Assets.Scripts
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(BoxCollider2D))]
    public class Controller2D : RaycastController
    {
        [SerializeField] public bool FacingRight = false;

        public LayerMask CollisionMask;

        public CollisionInfo Collisions;

        public SpriteRenderer SpriteRenderer;

        private const float Tolerance = 0.01f;

        public void Move(Vector3 velocity)
        {
            UpdateRaycastOrigins();
            Collisions.Reset();

            if (Math.Abs(velocity.x) >= Tolerance) HorizontalCollisions(ref velocity);
            if (Math.Abs(velocity.y) >= Tolerance) VerticalCollisions(ref velocity);

            if (velocity.x < 0 && !FacingRight)
            {
                Flip();
            }
            else if (velocity.x > 0 && FacingRight)
            {
                Flip();
            }

            transform.Translate(velocity);
        }

        public void Flip()
        {
            FacingRight = !FacingRight;
            //SpriteRenderer.flipX = !FacingRight;
            transform.localScale = new Vector3(FacingRight ? 1 : -1, 1, 1);
        }

        private void HorizontalCollisions(ref Vector3 velocity)
        {
            var directionY = Mathf.Sign(velocity.y);
            var directionX = Mathf.Sign(velocity.x);
            var rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for (var i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (int)directionX == -1 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                var hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, CollisionMask);

                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red, Time.deltaTime, false);

                if (!hit) continue;

                if ((int)directionY == 1 && hit.collider.tag == "Level")
                {
                    Collisions.Left = Collisions.Right = false;
                    continue;
                }

                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                Collisions.Left = (int)directionX == -1;
                Collisions.Right = (int)directionX == 1;
            }
        }

        private void VerticalCollisions(ref Vector3 velocity)
        {
            var directionY = Mathf.Sign(velocity.y);
            var rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (var i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (int)directionY == -1 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
                var hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, CollisionMask);

                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

                if (hit)
                {
                    if ((int)directionY == 1 && hit.collider.tag == "Level")
                    {
                        Collisions.Above = false;
                        continue;
                    }

                    velocity.y = (hit.distance - skinWidth) * directionY;
                    rayLength = hit.distance;

                    Collisions.Below = (int)directionY == -1;
                    Collisions.Above = (int)directionY == 1;
                }
            }
        }

        public struct CollisionInfo
        {
            public bool Above, Below;
            public bool Left, Right;

            public void Reset()
            {
                Above = Below = false;
                Left = Right = false;
            }
        }
    }
}