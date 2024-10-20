using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class PoolsManager : SingletonMonobehaviour<PoolsManager>
    {
        private Dictionary<GameObject, Pool> _pools = new Dictionary<GameObject, Pool>();

        internal Pool GetPool(GameObject prefab)
        {
            if (!_pools.ContainsKey(prefab)) _pools.Add(prefab, new Pool(prefab, 4, this.transform));
            return _pools[prefab];
        }            
    }
}
