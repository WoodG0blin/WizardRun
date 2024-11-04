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
    internal class AllItemConfigs : ScriptableObject, IDataSource<ItemConfig>
    {
        [SerializeField] private ItemConfig[] _configs;
        public IReadOnlyList<ItemConfig> Configs => _configs;
    }
}
