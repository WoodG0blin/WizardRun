using System.Collections;
using UnityEngine;
using WizardsPlatformer;

namespace WizardsPlatformer
{
    internal class ItemsRepository : Repository<string, IItem, ItemConfig>
    {
        public ItemsRepository(ItemConfig[] configs) : base(configs) { }
        protected override IItem CreateItem(ItemConfig config) => (IItem)config;
        protected override string GetKey(ItemConfig config) => config.ID;
    }
}