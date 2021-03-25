using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Random = System.Random;
using System;

namespace GuildMaster.Exploration.Events
{
    /// <summary>
    /// 인스턴스를 정수 가중치에 따라 랜덤으로 얻기 위한 class입니다.
    /// </summary>
    public class ProbabilityTool<T>
    {
        private Random _rnd;
        private List<Weighteditem> _elementlist;

        public ProbabilityTool(List<Weighteditem> elementlist, Random random)
        {
            _elementlist = elementlist.ToList<Weighteditem>();
            _rnd = random;
        }
        /// <summary>
        /// 멤버로 가지고 있는 _elementlist에서 랜덤한 요소를 뽑는 메서드입니다.
        /// </summary>
        public Weighteditem Getitem()
        {
            int totalWeight = 0;
            foreach (Weighteditem item in _elementlist)
            {
                totalWeight += item.Weight;
            }
            int randomNumber = _rnd.Next(0, totalWeight);

            Weighteditem selecteditem = null;
            bool flag = false;
            foreach (Weighteditem item in _elementlist)
            {
                if (randomNumber < item.Weight)
                {
                    flag = true;
                    selecteditem = item;
                    break;
                }

                randomNumber = randomNumber - item.Weight;
            }

            Assert.IsTrue(flag);

            return selecteditem;
        }
        /// <summary>
        /// ProbabilityTool 내의 중첩 클래스로, 생성자를 통해 인스턴스 본인과 정수 가중치를 받습니다.
        /// </summary>
        [Serializable] public class Weighteditem
        {
            public T item = default(T);
            public int Weight = 0;

            public Weighteditem(T n, int w)
            {
                this.item = n;
                this.Weight = w;
            }
        }
    }


}

