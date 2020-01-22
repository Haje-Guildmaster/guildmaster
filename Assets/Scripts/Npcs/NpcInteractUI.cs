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
        public Image Illustration => illustration;
        public Text DialogTextBox => dialogTextBox;

        public void Start()
        {
            gameObject.SetActive(false);
        }
        public void Open(NpcData npc)
        {
            _npcData = npc;
            InitialScreen();
            gameObject.SetActive(true);
        }
        
        public void Close()
        {
            gameObject.SetActive(false);
        }


        private NpcData _npcData;
        private void InitialScreen()
        {
            var data = _npcData.basicData;
            illustration.sprite = data.illustration;
            dialogTextBox.text = data.greeting;
        }

        private void InitializeInteractionList()
        {
            
        }

        private void AddInteractionToList()
        {
            
        }
    }
}