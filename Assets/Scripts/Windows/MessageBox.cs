using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Windows
{
    public class MessageBox: DraggableWindow
    {
        public Text titleLabel;
        public Text contentsLabel;
        public Transform buttonsParent;
        public Button buttonPrefab;
        
        protected override void OnClose()
        {
            Destroy(gameObject);
        }

        public void Open(string title, string content, IEnumerable<(string buttonText, Action onClicked)> buttons)
        {
            titleLabel.text = title;
            contentsLabel.text = content;

            foreach (Transform trs in buttonsParent)
            {
                Destroy(trs.gameObject);
            }

            foreach (var btn in buttons)
            {
                var made = Instantiate(buttonPrefab, buttonsParent);
                made.onClick.AddListener(Close);
                made.onClick.AddListener(()=>btn.onClicked());
                made.GetComponentInChildren<Text>().text = btn.buttonText;
            }
        }
        
    }
}