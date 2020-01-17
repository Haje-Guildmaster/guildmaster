using GuildMaster.TownRoam.TownModifiers;
using UnityEngine;

namespace GuildMaster.TownRoam
{
    public class TownModifierComponent : MonoBehaviour
    {
        [SerializeField] private Town town;
        private TownModifier _townLoader;

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