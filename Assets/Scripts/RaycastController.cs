namespace Assets.Scripts
{
    using System;
    using UnityEngine;

    public class RaycastController : MonoBehaviour
    {
        public float skinWidth = .025f;
        public int horizontalRayCount = 4;
        public int verticalRayCount = 4;

        [HideInInspector]
        protected float horizontalRaySpacing;
        [HideInInspector]
        protected float verticalRaySpacing;

        [HideInInspector]
        public BoxCollider2D BoxCollider2D;

        public RaycastOrigins raycastOrigins;

        void Start()
        {
            BoxCollider2D = GetComponent<BoxCollider2D>();
            CalculateRaySpacing();
        }

        protected void UpdateRaycastOrigins()
        {
            Bounds bounds = BoxCollider2D.bounds;
            bounds.Expand(skinWidth * -2);

            raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }

        protected void CalculateRaySpacing()
        {
            Bounds bounds = BoxCollider2D.bounds;
            bounds.Expand(skinWidth * -2);

            horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, Int32.MaxValue);
            verticalRayCount = Mathf.Clamp(verticalRayCount, 2, Int32.MaxValue);

            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }

        public struct RaycastOrigins
        {
            public Vector2 topLeft, topRight;
            public Vector2 bottomLeft, bottomRight;
        }
    }
}
