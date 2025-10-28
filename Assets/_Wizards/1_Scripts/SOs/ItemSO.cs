using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = "New" + nameof(ItemSO), menuName = "Configs/" + nameof(ItemSO), order = 2)]
    public class ItemSO : ScriptableObject, IDisplayInfo
    {
        [SerializeField] private ItemConfig _config;

        public string Name => _config.Name;

        public Sprite Icon => _config.Icon;

        public ItemConfig GetConfig() => _config.Clone();
    }

    [Serializable]
    public class ItemConfig : IEquatable<ItemConfig>
    {
        [field: SerializeField, HideInInspector] public string Hash { get; set; } = string.Empty;
        [field: SerializeField] public string NameTag { get; set; } = string.Empty;
        [field: SerializeField] public Sprite Icon { get; set; } = null;
        [field: SerializeField] public ArtifactSlotType SlotType { get; set; } = ArtifactSlotType.Universal;
        [field: SerializeField] public ArtifactPropertyConfig BaseProperty { get; set; } = null;
        [field: SerializeField] public List<ArtifactPropertyConfig> ExtraProperties { get; set; } = new();
        [field: SerializeField, HideInInspector] public int ControlIndex { get; set; } = -1;

        public string Name => NameTag;
        public bool HasExplicitProperty =>
            BaseProperty.ActivatorType == PropertyActivators.Explicit
            || ExtraProperties.Where(p => p.ActivatorType == PropertyActivators.Explicit).FirstOrDefault() != null;

        public ItemConfig Clone() => new()
        {
            NameTag = this.NameTag,
            Icon = this.Icon,
            SlotType = this.SlotType,
            BaseProperty = this.BaseProperty,
            ExtraProperties = this.ExtraProperties,
            ControlIndex = this.ControlIndex,
            Hash = $"{NameTag}+{DateTime.Now}"
        };

        public bool IsSame(ItemConfig other) => this.Hash == other.Hash;

        bool IEquatable<ItemConfig>.Equals(ItemConfig other) => this.Hash == other.Hash;

    }
}