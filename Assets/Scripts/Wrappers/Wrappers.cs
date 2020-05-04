namespace Assets.Scripts.Wrappers
{
    using UnityEngine;

    static class Wrappers
    {
        public static ITransform Wrap(this Transform unityObject) => new UnityTransform(unityObject);
    }

    public interface ITransform
    {
        Vector3 Position { get; set; }

        Vector3 LocalScale { get; set; }
    }
}
