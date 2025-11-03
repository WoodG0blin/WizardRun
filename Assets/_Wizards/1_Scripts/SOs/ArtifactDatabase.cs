using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IDataSource<T>
    {
        public IReadOnlyList<T> Configs { get; }
    }

    [CreateAssetMenu(fileName = nameof(ArtifactDatabase), menuName = "Configs/" + nameof(ArtifactDatabase), order = 4)]
    public class ArtifactDatabase : ScriptableObject
    {
        private static ArtifactDatabase _instance;

        [SerializeField] public ItemSO[] BaseArtifacts;
        [SerializeField] public ArtifactPropertySO[] BaseProperties;
        [SerializeField] public AmmoConfig[] Ammos;

        public void SetInstance() => _instance = this;

        public static AmmoConfig GetAmmoByName(string name) =>
            _instance.Ammos.Where(a => a.name == name).FirstOrDefault();
        public static Sprite GetArtifactSpriteByName(string nameTag)
        {
            Debug.Log($"trying to det sprite for {nameTag}. Has instance? {_instance != null}. Artifacts count {_instance.BaseArtifacts.Length}");
            var reference = _instance.BaseArtifacts.Where(a => a.NameTag == nameTag).FirstOrDefault();
            if (reference != null) return reference.Icon;
            else return null;
        }
    }

}
