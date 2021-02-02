using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GuildMaster.TownRoam
{
    public class MedicalBedCharacterSelectButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler,
        IPointerUpHandler
    {
        [SerializeField] private MedicalBedCharacterSelector _medicalBedCharacterSelector;


        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            _medicalBedCharacterSelector.OpenCharacterSelectDropdown(eventData.position);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Do nothing
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Do nothing
        }
    }
}