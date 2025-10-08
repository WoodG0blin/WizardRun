using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PlayerModel : IPlayerModel, IArtifactHolder
    {
        public LevelObjectConfig Config {get; private set; }

        private BonusStats _bonuses;
        private Dictionary<ArtifactSlotType, Artifact> _artifacts;
        
        public CharacterStats Stats { get; private set; }

        public string Name { get; private set; }
        public int Bonuses => _bonuses[BonusType.Coin];
        public PlayerSavedData SaveData { get; private set; }

        public List<IArtifact> EquippedArtifacts => _artifacts.Values.Where(a => a!=null).Cast<IArtifact>().ToList();
        //public List<ArtifactActor> ArtifactActors { get; private set; }
        //public Dictionary<Artifact.ArtifactActivatorTypes, List<ArtifactActor>> Actors { get; private set; }
        public ActionsHolder Actions { get; private set; }

        bool IArtifactHolder.IsPlayer => true;

        public PlayerModel(PlayerSavedData data, LevelObjectConfig config)
        {
            SaveData = data;

            Name = data.Name;

            Config = config;

            Stats = new(Config);

            _bonuses = new BonusStats(false);
            _bonuses[BonusType.Coin] = data.Bonuses;

            _artifacts = new()
            {
                { ArtifactSlotType.Weapon, null},
                { ArtifactSlotType.Head, null},
                { ArtifactSlotType.Neck, null},
                { ArtifactSlotType.Waist, null},
                { ArtifactSlotType.Legs, null}
            };

            //ArtifactActors = new();
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
                //UpdateActors();
                //Actions.SetActors(_artifacts.Values);
            }
            
            return res;
        }

        //private void UpdateActors()
        //{
        //    ArtifactActors = new(); //legacy
        //    Actors = new();

        //    List<ArtifactActor> modifiers = new();

        //    foreach (var art in _artifacts.Values)
        //        foreach (var act in art.Actors)
        //        {
        //            act.ClearAllModifiers();

        //            if (act.ActivatorType == Artifact.ArtifactActivatorTypes.Modifier) modifiers.Add(act);
        //            else
        //            {
        //                act.Set(ArtifactActors); //legacy

        //                if (Actors.ContainsKey(act.ActivatorType))
        //                {
        //                    if(act.IsMain) Actors[act.ActivatorType].Insert(0, act);
        //                    else Actors[act.ActivatorType].Add(act);
        //                }
        //                else Actors.Add(act.ActivatorType, new() { act });
        //            }
        //        }

        //    foreach (var mod in modifiers)
        //    {
        //        mod.Set(ArtifactActors); //legacy
        //        foreach(var list in Actors.Values) mod.Set(list);
        //    }
        //}

        public void AddBonus(BonusType type, int value)
        {
            _bonuses[type] += value;
            SaveData.Bonuses = _bonuses[BonusType.Coin];
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


        //LEGACY
        //private ArtifactActor _baseWeapon;

        //public Dictionary<Artifact.ArtifactActivatorTypes, List<ArtifactActor>> Actors { get; private set; } = new();
        //public ArtifactActor Weapon { get; private set; }

        //public ActionsHolder(ActorStatsConfig baseWeaponConfig)
        //{
        //    _baseWeapon = new AttackActor(baseWeaponConfig);
        //    Weapon = _baseWeapon;
        //}


        //public void SetActors(IEnumerable<Artifact> sourceArtifacts)
        //{
        //    Actors = new();

        //    List<ArtifactActor> modifiers = new();

        //    foreach (var art in sourceArtifacts)
        //        foreach (var act in art.Actors)
        //        {
        //            act.ClearAllModifiers();

        //            if (act.ActivatorType == Artifact.ArtifactActivatorTypes.Modifier) modifiers.Add(act);
        //            else
        //            {
        //                if (Actors.ContainsKey(act.ActivatorType))
        //                {
        //                    if (act.IsMain) Actors[act.ActivatorType].Insert(0, act);
        //                    else Actors[act.ActivatorType].Add(act);
        //                }
        //                else Actors.Add(act.ActivatorType, new() { act });
        //            }
        //        }

        //    foreach (var mod in modifiers)
        //    {
        //        foreach (var list in Actors.Values) mod.Set(list);
        //    }

        //    Weapon = _baseWeapon;
        //    if (Actors.ContainsKey(Artifact.ArtifactActivatorTypes.Attack) && Actors[Artifact.ArtifactActivatorTypes.Attack][0].IsMain)
        //    {
        //        Weapon = Actors[Artifact.ArtifactActivatorTypes.Attack][0];
        //        Actors[Artifact.ArtifactActivatorTypes.Attack].Remove(Weapon);
        //    }
        //}


        //public void SetHolder(IArtifactHolder holder)
        //{
        //    Weapon.SetHolder(holder);

        //    foreach(var list in Actors.Values)
        //        foreach(var act in list)
        //            act.SetHolder(holder);
        //}

        //public void TryUseForAction(Artifact.ArtifactActivatorTypes actionType)
        //{
        //    if (!Actors.ContainsKey(actionType)) return;

        //    foreach(var act in Actors[actionType])
        //    {
        //        if (act.IsReady) act.Use();
        //    }
        //}
    }
}