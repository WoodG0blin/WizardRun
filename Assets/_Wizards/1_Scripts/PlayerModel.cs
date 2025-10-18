using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerModel : IPlayerModel, IArtifactHolder
    {
        public LevelObjectConfig Config {get; private set; }

        public BonusStats Bonuses { get; private set; }
        private Dictionary<ArtifactSlotType, Artifact> _artifacts;
        
        public CharacterStats Stats { get; private set; }
        public int ModificationsCount { get; private set; } = 5;

        public string Name { get; private set; }
        public PlayerSavedData SaveData { get; private set; }

        public List<IArtifact> EquippedArtifacts => _artifacts.Values.Where(a => a!=null).Cast<IArtifact>().ToList();
        public ActionsHolder Actions { get; private set; }

        bool IArtifactHolder.IsPlayer => true;

        public PlayerModel(PlayerSavedData data, LevelObjectConfig config)
        {
            SaveData = data;

            Name = data.Name;

            Config = config;

            Stats = new(Config, data.BaseStatsModifiers);
            Stats.Modifiers.AddToAvailableModsCount(ModificationsCount);

            Bonuses = new BonusStats(false);
            Bonuses[BonusType.Coin] = data.Bonuses;

            _artifacts = new()
            {
                { ArtifactSlotType.Weapon, null},
                { ArtifactSlotType.Head, null},
                { ArtifactSlotType.Neck, null},
                { ArtifactSlotType.Waist, null},
                { ArtifactSlotType.Legs, null}
            };

            Actions = new(this);
            ArtifactProperty _weapon = new(config.WeaponConfig, Name, isBaseProperty: true);
            _weapon.Init(this);
        }


        public bool TrySetArtifactAt(ArtifactSlotType slot, ItemConfig artifact)
        {
            bool res =
                artifact != null ?
                slot == artifact.SlotType : true;

            // conditions to equip
            if (res)
            {
                _artifacts[slot]?.Unequip();

                _artifacts[slot] = null;

                if(artifact != null)
                {
                    var a = new Artifact(artifact);
                    _artifacts[slot] = a;
                    a.Equip(this);
                }
            }
            
            return res;
        }

        public void AddBonus(BonusType type, int value)
        {
            Bonuses[type] += value;
            SaveData.Bonuses = Bonuses[BonusType.Coin];
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
}