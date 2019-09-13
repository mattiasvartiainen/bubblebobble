namespace Assets.Scripts
{
    using UnityEngine;

    [RequireComponent(typeof(BoxCollider2D))]
    public class Controller2D : RaycastController
    {
        [SerializeField] public bool FacingRight = false;

        public LayerMask collisionMask;

        public CollisionInfo collisions;

        public SpriteRenderer SpriteRenderer;

        public void Move(Vector3 velocity)
        {
            UpdateRaycastOrigins();
            collisions.Reset();

            if (velocity.x != 0) HorizontalCollisions(ref velocity);
            if (velocity.y != 0) VerticalCollisions(ref velocity);

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

        private void Flip()
        {
            FacingRight = !FacingRight;
            SpriteRenderer.flipX = !FacingRight;
        }

        private void HorizontalCollisions(ref Vector3 velocity)
        {
            var directionY = Mathf.Sign(velocity.y);
            var directionX = Mathf.Sign(velocity.x);
            var rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for (var i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = directionX == -1 ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                var hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

                if (hit)
                {
                    if (directionY == 1 && hit.collider.tag == "Level")
                    {
                        collisions.left = collisions.right = false;
                        continue;
                    }

                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }

        private void VerticalCollisions(ref Vector3 velocity)
        {
            var directionY = Mathf.Sign(velocity.y);
            var rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for (var i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = directionY == -1 ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
                var hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

                if (hit)
                {
                    if (directionY == 1 && hit.collider.tag == "Level")
                    {
                        collisions.above = false;
                        continue;
                    }

                    velocity.y = (hit.distance - skinWidth) * directionY;
                    rayLength = hit.distance;

                    collisions.below = directionY == -1;
                    collisions.above = directionY == 1;
                }
            }
        }

        public struct CollisionInfo
        {
            public bool above, below;
            public bool left, right;

            public void Reset()
            {
                above = below = false;
                left = right = false;
            }
        }
    }
}