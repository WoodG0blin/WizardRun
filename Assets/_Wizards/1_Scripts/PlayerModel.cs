using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerModel : IPlayerModel, IArtifactHolder
    {
        private PlayerSavedData _saveData;

        public LevelObjectConfig Config {get; private set; }

        public MasteryData Mastery { get; private set; } = new();

        public Dictionary<BonusType, int> Bonuses { get; private set; }
        private Dictionary<ArtifactSlotType, Artifact> _artifacts;
        
        public CharacterStats Stats { get; private set; }
        public int ModificationsCount { get; private set; } = 5;

        public string Name => _saveData.Name;
        public Sprite Icon { get; private set; }

        public List<IArtifact> EquippedArtifacts => _artifacts.Values.Where(a => a!=null).Cast<IArtifact>().ToList();
        public List<ItemConfig> Chest { get; private set; } = new();
        public int MaxInventorySlots { get; private set; } = 10;
        public ActionsHolder Actions { get; private set; }

        public Action OnValuesChanged { get; set; }

        bool IArtifactHolder.IsPlayer => true;

        public PlayerModel(PlayerSavedData data, LevelObjectConfig config)
        {
            _saveData = data;

            Config = config;

            Stats = new(Config, data.BaseStatsModifiers);
            Stats.Modifiers.AddToAvailableModsCount(ModificationsCount);

            Bonuses = new()
            {
                { BonusType.Coin, 0 },
                { BonusType.Souls, 0},
                { BonusType.Artifacts, 0}
            };
            foreach(var b in data.BonusStats)
                Bonuses[b.Type]= b.Value;
            Mastery = new(data.MasteryLevel);

            Actions = new(this);
            ArtifactProperty _weapon = new(config.WeaponConfig, null, 0);
            _weapon.Init(this);

            _artifacts = new()
            {
                { ArtifactSlotType.Weapon, null},
                { ArtifactSlotType.Head, null},
                { ArtifactSlotType.Neck, null},
                { ArtifactSlotType.Waist, null},
                { ArtifactSlotType.Legs, null},
                { ArtifactSlotType.Ring, null}
            };
            foreach(var item in data.EquipedArtifacts)
                if(item != null) EquipArtifact(item.SlotType, item);
            
            Chest = new();
            foreach (var item in data.ChestArtifacts)
                AddArtifact(item);

        }

        public void EquipArtifact(ArtifactSlotType slot, ItemConfig artifact)
        {
            if (_artifacts[slot] != null)
            {
                if(_artifacts[slot].Config != artifact)
                    Chest.Add(_artifacts[slot].Config);
                _artifacts[slot].Unequip();
            }

            _artifacts[slot] = null;

            if(artifact != null)
            {
                if(Chest.Contains(artifact)) Chest.Remove(artifact);
                var a = new Artifact(artifact);
                _artifacts[slot] = a;
                a.Equip(this);
            }

            OnValuesChanged?.Invoke();
        }

        public void AddArtifact(ItemConfig artifact)
        {
            Chest.Add(artifact);
            OnValuesChanged?.Invoke();
        }

        public void AddBonus(BonusType type, int value)
        {
            if(!Bonuses.ContainsKey(type)) Bonuses.Add(type, 0);
            Bonuses[type] += value;
            OnValuesChanged?.Invoke();
        }

        public void SetNewDisplayName(string newName)
        {
            _saveData.Name = newName;
            OnValuesChanged?.Invoke();
            Stats.OnBaseParametersChange?.Invoke();
        }

        public PlayerSavedData GetSaveData()
        {
            _saveData.EquipedArtifacts = EquippedArtifacts.Select(a => a.Config).ToList();
            _saveData.ChestArtifacts = Chest;

            _saveData.BonusStats = new();
            foreach(var kvp in Bonuses)
                _saveData.BonusStats.Add(new(kvp.Key, kvp.Value));
            _saveData.MasteryLevel = Mastery.Current;

            _saveData.BaseStatsModifiers = new();
            foreach (var kvp in Stats.Modifiers.BaseModifiers)
                _saveData.BaseStatsModifiers.Add(new(kvp.Key, kvp.Value));

            return _saveData;
        }
    }

    public class ActionsHolder
    {
        //NEW

        private Dictionary<PropertyActivators, List<IArtifactExecutor>> _availableActions = new();

        public ActionsHolder(IArtifactHolder owner) { }

        public void AddAction(PropertyActivators activatorType, IArtifactExecutor action, bool isBase = false)
        {
            if (!_availableActions.ContainsKey(activatorType)) _availableActions.Add(activatorType, new());
            
            if(isBase) _availableActions[activatorType].Insert(0, action);
            else _availableActions[activatorType].Add(action);
        }

        public List<IArtifactExecutor> GetActionsFor(PropertyActivators activatorType)
        {
            if (_availableActions.ContainsKey(activatorType)) return _availableActions[activatorType];
            else return new();
        }

        public void RemoveAction(PropertyActivators activatorType, IArtifactExecutor action)
        {
            if (!_availableActions.ContainsKey(activatorType)) return;
            if (!_availableActions[activatorType].Contains(action)) return;
            _availableActions[activatorType].Remove(action);
        }
    }

    [Serializable]
    public class MasteryData
    {
        private List<int> gradesThresholds = new()
        {
            10,
            50,
            100,
            200,
            500,
            1000
        };

        public MasteryData() : this(0) { }
        public MasteryData(int initial) => Change(initial);


        public int Current;
        public int Grade { get; private set; }
        public int MaxLevelForGrade { get; private set; }

        public void Change(int change)
        {
            Current = Mathf.Max(Current + change, 0);
            Grade = GetCurrentGrade();
            MaxLevelForGrade = (gradesThresholds.Count < Grade) ? gradesThresholds[Grade] : 0;
        }

        private int GetCurrentGrade()
        {
            for (int i = 0; i < gradesThresholds.Count; i++)
            {
                if (Current < gradesThresholds[i]) return i;
            }
            return gradesThresholds.Count;
        }
    }
}