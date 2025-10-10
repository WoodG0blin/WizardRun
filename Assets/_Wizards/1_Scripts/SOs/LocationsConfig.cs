using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;


namespace WizardsPlatformer
{
    public enum LocationType
    {
        Neutral = 0,
        Forest = 1,
        Stone = 2,
        Fire = 3,
        Wind = 4
    }

    [CreateAssetMenu(fileName = nameof(LocationsConfig), menuName = "Configs/" + nameof(LocationsConfig))]
    public class LocationsConfig : ScriptableObject
    {
        [SerializeField] private List<LocationConfig> _locations = new();

        public LocationConfig GetRandomLocation() =>
            _locations[UnityEngine.Random.Range(0, _locations.Count)];

        public LocationConfig LoadLocation(LocationType type) =>
            _locations.Where(c => c.Type == type).FirstOrDefault();
    }

    [Serializable]
    public class LocationConfig
    {
        public LocationType Type;
        public List<Sprite> Images = new();
        public LevelConfig LevelConfig;

        public string SetSprite() =>
            Images[UnityEngine.Random.Range(0, Images.Count)].name;

        public Sprite GetSprite(string ID) =>
            Images.Where(i => i.name == ID).FirstOrDefault();
    }
}
