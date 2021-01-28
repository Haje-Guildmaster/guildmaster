using GuildMaster.Data;
using UnityEngine;

namespace GuildMaster.TownRoam
{
    /// <summary>
    /// 맵에 이 오브젝트가 존재할 시, 시간이 한 단계 흐를 때마다 Player.Instance.MedicalBed 안의 캐릭터들을 치유합니다.
    /// </summary>
    public class Healer : MonoBehaviour
    {
        [SerializeField] private int _hpHealPerTime;
        [SerializeField] private int _stmHealPerTime;

        public readonly MedicalBed TargetMedicalBed = Player.Instance.MedicalBed;

        private void Awake()
        {
            _timeManagerCache = Player.Instance.TimeManager;
        }

        private void OnEnable()
        {
            _timeManagerCache.TimeChanged += Heal;
#if UNITY_EDITOR
            int length;
            if ((length = FindObjectsOfType<Healer>().Length) > 1)
            {
                Debug.LogWarning($"There is more than 1 healers({length}) in this scene. " +
                                 $"If this is what you intended, you can ignore this warning");
            }
#endif
        }

        private void OnDisable()
        {
            _timeManagerCache.TimeChanged -= Heal;
        }

        private void Heal(int timeChangedAmount)
        {
            foreach (var ch in TargetMedicalBed.OnBeds)
            {
                if (ch == null) continue;
                ch.Hp += timeChangedAmount * _hpHealPerTime;
                ch.Stamina += timeChangedAmount * _stmHealPerTime;
            }
        }

        private Timemanagement _timeManagerCache;
    }
}