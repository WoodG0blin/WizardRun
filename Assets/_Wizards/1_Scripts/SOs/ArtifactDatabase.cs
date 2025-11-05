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

        public static AmmoConfig GetAmmoByName(string name)
        {
            var res = _instance.Ammos.Where(a => a.name == name).FirstOrDefault();
            res ??= _instance.BaseArtifacts.Select(a => a.GetConfig().BaseProperty.Ammo).Where(ammo => ammo != null && ammo.name == name).FirstOrDefault();
            return res;
        }
        public static Sprite GetArtifactSpriteByName(string nameTag)
        {
            var reference = _instance.BaseArtifacts.Where(a => a.NameTag == nameTag).FirstOrDefault();
            if (reference != null) return reference.Icon;
            else return null;
        }
    }

}
