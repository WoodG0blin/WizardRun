using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IDataSource<T>
    {
        public IReadOnlyList<T> Configs { get; }
    }

    [CreateAssetMenu(fileName = nameof(AllItemConfigs), menuName = "Configs/" + nameof(AllItemConfigs), order = 4)]
    internal class AllItemConfigs : ScriptableObject, IDataSource<ItemSO>
    {
        [SerializeField] private ItemSO[] _configs;
        public IReadOnlyList<ItemSO> Configs => _configs;
    }
}
