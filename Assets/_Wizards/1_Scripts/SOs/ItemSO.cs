using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    [CreateAssetMenu(fileName = "New" + nameof(ItemSO), menuName = "Configs/" + nameof(ItemSO), order = 2)]
    public class ItemSO : ScriptableObject, IDisplayInfo
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [SerializeField] private ItemConfig _config;
        [SerializeField] private AmmoConfig _baseAmmo;
        public string Name => _config.Name;
        public string NameTag => _config.NameTag;
        //public Sprite Icon => _config.Icon;

        public ItemConfig GetConfig()
        {
            var res = _config.Clone();
            res.SetIcon(Icon);
            res.BaseProperty.SetAmmo(_baseAmmo);
            return res;
        }
    }

    [Serializable]
    [Newtonsoft.Json.JsonConverter(typeof(ItemConfigJSONConverter))]
    public class ItemConfig : IEquatable<ItemConfig>
    {
        [field: SerializeField, HideInInspector] public string Hash { get; set; } = string.Empty;
        [field: SerializeField] public string NameTag { get; set; } = string.Empty;
        [field: SerializeField, HideInInspector] public string IconReference { get; set; } = string.Empty;
        [field: SerializeField] public ArtifactSlotType SlotType { get; set; } = ArtifactSlotType.Universal;
        [field: SerializeField] public ArtifactPropertyConfig BaseProperty { get; set; } = null;
        [field: SerializeField, HideInInspector] public List<ArtifactPropertyConfig> ExtraProperties { get; set; } = new();
        [field: SerializeField, HideInInspector] public int ControlIndex { get; set; } = -1;

        public string Name => NameTag;
        public Sprite Icon { get; private set; }
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

        public void SetIcon(Sprite icon) => Icon = icon;

        public void LoadResources()
        {
            SetIcon(ArtifactDatabase.GetArtifactSpriteByName(NameTag));
            BaseProperty.LoadResources();
            foreach (var p in ExtraProperties) p.LoadResources();
        }

        public bool IsSame(ItemConfig other) => this.Hash == other.Hash;

        bool IEquatable<ItemConfig>.Equals(ItemConfig other) => this.Hash == other.Hash;

    }

    public class ItemConfigJSONConverter : JsonConverter<ItemConfig>
    {
        public override ItemConfig ReadJson(JsonReader reader, Type objectType, ItemConfig existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var res = existingValue ?? new ItemConfig();
            if (reader == null) return res;
            
            JObject values = JObject.Load(reader);

            res = JsonUtility.FromJson<ItemConfig>(values.ToString());

            var sprite = ArtifactDatabase.GetArtifactSpriteByName(res.NameTag);
            Debug.Log($"Trying to get icon by {res.NameTag}. Icon name is {sprite}");
            res.SetIcon(sprite);
            return res;
        }

        public override void WriteJson(JsonWriter writer, ItemConfig value, JsonSerializer serializer)
        {
            //JObject data = new();

            //data.Add("Hash", value.Hash);
            //data.Add("NameTag", value.NameTag);
            //data.Add("SlotType", (int)value.SlotType);
            //data.Add("BaseProperty", value.BaseProperty);
            //data.Add("Hash", value.Hash);
            //data.Add("Hash", value.Hash);
        }
    }
}