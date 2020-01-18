using GuildMaster.TownRoam.TownModifiers;
using GuildMaster.TownRoam.Towns;
using UnityEngine;

namespace GuildMaster.TownRoam.TownModifiers
{
    public class TownModifierComponent : MonoBehaviour
    {
        [SerializeField] private Town town;
        private readonly TownModifier _townLoader;

        public TownModifierComponent(TownModifier townLoader)
        {
            _townLoader = townLoader;
        }

        public TownModifierComponent(): this(new BasicTownModifier()) {}
        
        private void OnEnable()
        {
            _townLoader.Modify(town);
        }
    }
}