using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsPlatformer;

[CreateAssetMenu(fileName = "New" + nameof(AmmoConfig), menuName = "Configs/" + nameof(AmmoConfig))]
public class AmmoConfig : ScriptableObject
{
    [field: SerializeField] public AmmoType Type { get; set; }
    [field: SerializeField, Min(0)] public int ActionRange { get; set; }
    [field: SerializeField, Min(0)] public int ActionSpeed { get; set; }
    [field: SerializeField] public GameObject Prefab { get; set; }
}


public enum AmmoType
{
    Melee = 0,
    Explosion = 1,

    SimpleRanged = 10,
    Ballistic = 11,

    Missile = 20
}

public class AmmoConfigJSONConverter : JsonConverter<AmmoConfig>
{
    public override AmmoConfig ReadJson(JsonReader reader, Type objectType, AmmoConfig existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        Debug.Log("Deserializing ammo comfig");
        string name = reader.Value.ToString();
        return ArtifactDatabase.GetAmmoByName(name);
    }

    public override void WriteJson(JsonWriter writer, AmmoConfig value, JsonSerializer serializer)
    {
        writer.WriteValue(value.name);
    }
}
