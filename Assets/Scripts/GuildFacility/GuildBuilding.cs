using System;
using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster.GuildFacility
{
    public class GuildBuilding
    {
        private int curFloor;
        private readonly int maxHo = 5;
        private readonly int maxFloor = 5;
        private GuildFacility[,] facilitys;

        public GuildBuilding()
        {
            // TODO : save/load
            curFloor = 2;

            facilitys = new GuildFacility[maxFloor, maxHo];
        }

        public void SetFacility(GuildFacility facility, int floor, int ho)
        {
            if (!IsValidLocation(floor, ho))
            {
                Debug.LogError($"존재하지 않는 방 {floor}층 {ho}호");
                return;
            }
            if (!IsUnlockLocation(floor, ho))
            {
                Debug.LogError($"해금되지 않은 방 {floor}층 {ho}호");
                return;
            }
            if (facilitys[floor, ho] != null)
            {
                Debug.LogWarning($"비어있지 않은 방에 시설 덮어씌우려는 시도 {floor}층 {ho}호");
                return;
            }
            facilitys[floor, ho] = facility;
        }

        public void removeFacility(int floor, int ho)
        {
            if (!IsValidLocation(floor, ho))
            {
                Debug.LogError($"존재하지 않는 방 {floor}층 {ho}호");
                return;
            }
            if (!IsUnlockLocation(floor, ho))
            {
                Debug.LogError($"해금되지 않은 방 {floor}층 {ho}호");
                return;
            }
            if (facilitys[floor, ho] == null)
            {
                Debug.LogWarning($"비어있는 방을 비우는 시도 {floor}층 {ho}호");
                return;
            }
            facilitys[floor, ho] = null;

        }

        private bool IsValidLocation(int floor, int ho)
        {
            if (floor < 0 || floor >= maxFloor)
                return false;
            if (ho < 0 || ho >= maxHo)
                return false;
            return true;
        }

        private bool IsUnlockLocation(int floor, int ho)
        {
            if (floor < 0 || floor >= curFloor)
                return false;
            if (ho < 0 || ho >= maxHo)
                return false;
            return true;
        }
    }
}