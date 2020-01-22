using System.Security.Cryptography.X509Certificates;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Npcs
{
    public class NpcInteractUI: MonoBehaviour
    {
        [SerializeField] private Image illustration;
        [SerializeField] private Text dialogTextBox;

        public void Start()
        {
            gameObject.SetActive(false);
        }
        public void Open(NpcData npc)
        {
            var data = npc.basicData;
            illustration.sprite = data.illustration;
            dialogTextBox.text = data.greeting;
            gameObject.SetActive(true);
        }
        public void Close()
        {
            gameObject.SetActive(false);
        }
        

    }
}