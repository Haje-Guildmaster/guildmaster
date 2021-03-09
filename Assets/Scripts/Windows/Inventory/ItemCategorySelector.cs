using System;
using GuildMaster.Data;
using UnityEngine;

namespace GuildMaster.Windows
{
    public class ItemCategorySelector: MonoBehaviour
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
                ict.Toggle.onValueChanged.AddListener(b =>
                {
                    if (b) CategoryChanged?.Invoke(cat);
                });
            }
        }
        
        public void SetCategory(PlayerInventory.ItemCategory category)
        {
            foreach (var ict in _childTogglesCache)
            {
                ict.Toggle.SetIsOnWithoutNotify(ict.category == category);
            }
        }

        private ItemCategoryToggle[] _childTogglesCache;
    }
}