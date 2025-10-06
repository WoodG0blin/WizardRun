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

        //public LocationType GetRandomLocationType()
        //{
        //    var locs = Enum.GetValues(typeof(LocationType)).Cast<LocationType>().ToList();
        //    return locs[UnityEngine.Random.Range(1, locs.Count)];
        //}

        //public Sprite GetRandomLocationImage(LocationType type)
        //{
        //    var pool = _locations.Where(t => t.Type == type).FirstOrDefault();
        //    if(pool != null) return pool.Images[UnityEngine.Random.Range(0, pool.Images.Count)];
        //    else return _locations[0].Images[UnityEngine.Random.Range(0, _locations[0].Images.Count)];
        //}

        //public Sprite GetLocationImage(LocationType type, string id)
        //{
        //    var pool = _locations.Where(t => t.Type == type).FirstOrDefault();
        //    var img = (pool != null) ?
        //        pool.Images.Where(i => i.name == id).FirstOrDefault() :
        //        _locations[0].Images[UnityEngine.Random.Range(0, _locations[0].Images.Count)];
        //    return img;
        //}
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
