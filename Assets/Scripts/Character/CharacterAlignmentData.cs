using UnityEngine;
using UnityEditor;

namespace GuildMaster.Characters
{
    public class CharacterAlignmentData
    {
        [SerializeField] private int lawOrChaos;
        [SerializeField] private int goodOrEvil;
        [SerializeField] private int smartOrStupid;

        private const int low = 0;
        private const int high = 7;
        
        public void GoLaw(int num)
        {
            int temp = lawOrChaos - num;
            if (temp < low) lawOrChaos = low;
            else lawOrChaos = temp;
        }
        public void GoChaos(int num)
        {
            int temp = lawOrChaos + num;
            if (temp > high) lawOrChaos = high;
            else lawOrChaos = temp;
        }
        public void GoGood(int num)
        {
            int temp = goodOrEvil - num;
            if (temp < low) goodOrEvil = low;
            else goodOrEvil = temp;
        }
        public void GoEvil(int num)
        {
            int temp = goodOrEvil + num;
            if (temp > high) goodOrEvil = high;
            else goodOrEvil = temp;
        }
        public void GoSmart(int num)
        {
            int temp = smartOrStupid - num;
            if (temp < low) smartOrStupid = low;
            else smartOrStupid = temp;
        }
        public void GoStupid(int num)
        {
            int temp = smartOrStupid + num;
            if (temp > high) smartOrStupid = high;
            else smartOrStupid = temp;
        }
    }
}