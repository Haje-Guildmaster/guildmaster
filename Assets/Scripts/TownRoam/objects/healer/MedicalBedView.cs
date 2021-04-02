using System;
using System.Linq;
using System.Threading.Tasks;
using GuildMaster.Characters;
using GuildMaster.Data;
using GuildMaster.Windows;
using TMPro;
using UnityEngine;

namespace GuildMaster.TownRoam
{
    /// <summary>
    /// 치료 중인 캐릭터 하나를 보여주는 오브젝트.
    /// </summary>
    public class MedicalBedView : MonoBehaviour
    {
        [SerializeField] private int _bedIndex;
        [SerializeField] private AutoRefreshedCharacterView _characterView;

        private async void OnEnable()
        {
            _medicalBedCache = Player.Instance.MedicalBed;
            _medicalBedCache.Changed += Refresh;
            await Task.Yield();     // Refresh에서 쓰이는 다른 오브젝트들의 Awake가 불리기를 기다리기 위해 1프레임 기다림
            Refresh();
        }

        private void OnDisable()
        {
            _medicalBedCache.Changed -= Refresh;
            _medicalBedCache = null;
        }


        private void Refresh()
        {
            _currentCharacter = _medicalBedCache.OnBeds[_bedIndex];
            _characterView.SetCharacter(_currentCharacter);
        }
        // private void OpenChangeCharacterDropdown

        private MedicalBed _medicalBedCache;
        private Character _currentCharacter;
    }
}