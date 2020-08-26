using System;
using UnityEngine;

namespace GuildMaster.Characters
{
    [Serializable]
    public struct CharacterAlignmentData
    {
        public int LawOrChaos;
        public int GoodOrEvil;
        public int SmartOrStupid;

        private const int Low = 0;
        private const int High = 7;
        
        public void GoLaw(int num)
        {
            int temp = LawOrChaos - num;
            if (temp < Low) LawOrChaos = Low;
            else LawOrChaos = temp;
        }
        public void GoChaos(int num)
        {
            int temp = LawOrChaos + num;
            if (temp > High) LawOrChaos = High;
            else LawOrChaos = temp;
        }
        public void GoGood(int num)
        {
            int temp = GoodOrEvil - num;
            if (temp < Low) GoodOrEvil = Low;
            else GoodOrEvil = temp;
        }
        public void GoEvil(int num)
        {
            int temp = GoodOrEvil + num;
            if (temp > High) GoodOrEvil = High;
            else GoodOrEvil = temp;
        }
        public void GoSmart(int num)
        {
            int temp = SmartOrStupid - num;
            if (temp < Low) SmartOrStupid = Low;
            else SmartOrStupid = temp;
        }
        public void GoStupid(int num)
        {
            int temp = SmartOrStupid + num;
            if (temp > High) SmartOrStupid = High;
            else SmartOrStupid = temp;
        }
    }
}