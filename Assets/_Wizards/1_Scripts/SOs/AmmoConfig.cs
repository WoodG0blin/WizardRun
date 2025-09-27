using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WizardsPlatformer;

[CreateAssetMenu(fileName = "New" + nameof(AmmoConfig), menuName = "Configs/" + nameof(AmmoConfig))]
public class AmmoConfig : ScriptableObject
{
    [field: SerializeField] public AmmoType Type { get; set; }
    [field: SerializeField, Min(0)] public int ActionRange { get; set;}
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
