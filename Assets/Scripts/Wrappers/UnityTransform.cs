namespace Assets.Scripts.Wrappers
{
    using UnityEngine;

    public class UnityTransform : ITransform
    {
        private readonly Transform _transform;

        public UnityTransform(Transform transform)
        {
            this._transform = transform;
        }

        public Vector3 Position
        {
            get => _transform.position;
            set => _transform.position = value;
        }

        public Vector3 LocalScale
        {
            get => _transform.localScale; 
            set => _transform.localScale = value;
        }
    }
}
