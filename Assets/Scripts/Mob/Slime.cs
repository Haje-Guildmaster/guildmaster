using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GuildMaster
{
    public class Slime : MobBase
    {
        public override int HP { get; set; }
        public override int ATK { get; set; }
        public override int DEF { get; set; }
        public override int SPD { get; set; }

        // Initialize Slime's stat
        public override void Init()
        {
            this.HP = 5;
            this.ATK = 3;
            this.DEF = 1;
            this.SPD = 3;
        }

        // Slime's Skill Erode
        public void Erode(CharacterBase enemy)
        {
            enemy.GetDamage(this.ATK);
            Debug.Log("Slime use Skill, Erode");
        }

        // Slime's AI
        public override void AI()
        {
            //Erode();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}