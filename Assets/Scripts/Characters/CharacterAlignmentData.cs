using System;
using UnityEngine;

namespace GuildMaster.Characters
{
    [Serializable]
    public struct CharacterAlignmentData
    {
        public int lawOrChaos;
        public int goodOrEvil;
        public int smartOrStupid;

        private const int Low = 0;
        private const int High = 7;
        
        public void GoLaw(int num)
        {
            int temp = lawOrChaos - num;
            if (temp < Low) lawOrChaos = Low;
            else lawOrChaos = temp;
        }
        public void GoChaos(int num)
        {
            int temp = lawOrChaos + num;
            if (temp > High) lawOrChaos = High;
            else lawOrChaos = temp;
        }
        public void GoGood(int num)
        {
            int temp = goodOrEvil - num;
            if (temp < Low) goodOrEvil = Low;
            else goodOrEvil = temp;
        }
        public void GoEvil(int num)
        {
            int temp = goodOrEvil + num;
            if (temp > High) goodOrEvil = High;
            else goodOrEvil = temp;
        }
        public void GoSmart(int num)
        {
            int temp = smartOrStupid - num;
            if (temp < Low) smartOrStupid = Low;
            else smartOrStupid = temp;
        }
        public void GoStupid(int num)
        {
            int temp = smartOrStupid + num;
            if (temp > High) smartOrStupid = High;
            else smartOrStupid = temp;
        }
    }
}