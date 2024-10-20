using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace WizardsPlatformer
{
    public interface IRepository : IDisposable { }

    internal abstract class Repository<TKey, TValue, TConfig> : IRepository
    {
        private Dictionary<TKey, TValue> _items;
        public IReadOnlyDictionary<TKey, TValue> Items => _items;
        protected Repository(IEnumerable<TConfig> configs) => _items = FillRepository(configs);

        private Dictionary<TKey, TValue> FillRepository(IEnumerable<TConfig> configs)
        {
            var temp = new Dictionary<TKey, TValue>();
            foreach (TConfig config in configs)
                temp.Add(GetKey(config), CreateItem(config));
            return temp;
        }

        protected abstract TValue CreateItem(TConfig config);
        protected abstract TKey GetKey(TConfig config);

        public void Dispose()
        {
            foreach(TValue item in _items.Values)
                if(item is IDisposable i) i.Dispose();
            _items.Clear();
        }
    }
}
