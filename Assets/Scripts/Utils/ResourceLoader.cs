using System;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal static class ResourceLoader
    {
        public static Sprite LoadSprite(string path) =>
            Resources.Load<Sprite>(path);

        public static GameObject LoadPrefab(string path) =>
            Resources.Load<GameObject>(path);

        public static View LoadView(string path) =>
            Resources.Load<View>(path);

        public static TObject Load<TObject>(string path) where TObject : UnityEngine.Object
        {
            return Resources.Load<TObject>(path);
        }

        public static TResult[] LoadFromDataSource<TSource, TResult>(string path) where TSource : ScriptableObject, IDataSource<TResult> where TResult : ScriptableObject
        {
            IDataSource<TResult> source = Resources.Load<TSource>(path);
            return source == null ? Array.Empty<TResult>() : source.Configs.ToArray();
        }
    }
}
