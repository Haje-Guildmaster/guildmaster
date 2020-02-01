using System;
using System.Security.Cryptography.X509Certificates;
using GuildMaster.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GuildMaster.Npcs
{
    public class NpcInteractUI: MonoBehaviour
    {
        [SerializeField] private Image illustration;
        [SerializeField] private Text dialogTextBox;
        [SerializeField] private RectTransform interactionButtonsParent;
        [SerializeField] private NpcInteractionButton interactionButtonPrefab;
        
        public Image Illustration => illustration;
        public Text DialogTextBox => dialogTextBox;

        public void Start()
        {
            gameObject.SetActive(false);
        }
        public void Open(NpcData npc)
        {
            _npcData = npc;
            gameObject.SetActive(true);
            InitialScreen();
        }
        
        public void Close()
        {
            Debug.Log("ASDFASDF");
            gameObject.SetActive(false);
        }


        private NpcData _npcData;
        private float _interactionListBottom;
        private const float InteractionButtonYDiff = 43f;
        private void InitialScreen()
        {
            illustration.GetComponent<YouSpinMeRound>().x = 0;
            illustration.GetComponent<YouSpinMeRound>().y = 0;
            illustration.GetComponent<YouSpinMeRound>().z = 0;
            
            
            var basicData = _npcData.basicData;
            illustration.sprite = basicData.illustration;
            dialogTextBox.text = $"[{basicData.npcName}]\n{basicData.greeting}";
            InitializeInteractionButtonList();
        }

        private void InitializeInteractionButtonList()
        {
            foreach (Transform child in interactionButtonsParent)
                Destroy(child.gameObject);

            _interactionListBottom = 0f;
            AddInteractionButtonToList("대화하기", ()=>
            {
                dialogTextBox.text = "(들리지 않는 것 같다..)";
                illustration.GetComponent<YouSpinMeRound>().z -= 20;
            });
            AddInteractionButtonToList("거래", ()=>
            {
                dialogTextBox.text = "(들리지 않는 것 같다...)";
                illustration.GetComponent<YouSpinMeRound>().x += 20;
                illustration.GetComponent<YouSpinMeRound>().y += 20;
            });
            if (_npcData.HasQuests) ;
            // AddInteractionButtonToList();
            AddInteractionButtonToList("때리기", () =>
            {
                dialogTextBox.text = "아야!";
                illustration.GetComponent<YouSpinMeRound>().x *= 2;
                illustration.GetComponent<YouSpinMeRound>().y *= 2;
                illustration.GetComponent<YouSpinMeRound>().z *= 2;
            });
        }

        private void AddInteractionButtonToList(string buttonText, UnityAction handler)
        {
            var button = Instantiate(interactionButtonPrefab, interactionButtonsParent);
            button.button.onClick.AddListener(handler);
            button.buttonText.text = buttonText;
            button.transform.localPosition += new Vector3(0, _interactionListBottom, 0);
            _interactionListBottom -= InteractionButtonYDiff;
        }
    }
}