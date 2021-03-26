using System;
using GuildMaster.Data;
using UnityEngine;

namespace GuildMaster.Windows
{
    public class ItemCategorySelector : MonoBehaviour
    {
        public event Action<PlayerInventory.ItemCategory> CategoryChanged;

        private void Awake()
        {
            _childTogglesCache = GetComponentsInChildren<ItemCategoryToggle>();
        }

        private void Start()
        {
            foreach (var ict in _childTogglesCache)
            {
                var cat = ict.category;
                ict.ValueChanged += b =>
                {
                    if (b) CategoryChanged?.Invoke(cat);
                };
            }
        }

        public void SetCategoryWithoutNotify(PlayerInventory.ItemCategory category)
        {
            foreach (var ict in _childTogglesCache)
            {
                ict.SetIsOnWithoutNotify(ict.category == category);
            }
        }

        private ItemCategoryToggle[] _childTogglesCache;
    }
}