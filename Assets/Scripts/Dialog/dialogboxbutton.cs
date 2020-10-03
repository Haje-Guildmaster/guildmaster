using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GuildMaster.Dialogs
{
    public class dialogboxbutton : MonoBehaviour
    {
        [SerializeField] private Windows.DialogUI dialogparent;
        Button button;
        public void OnClickButton()
        {

            dialogparent.Printtext("다음 문장");

        }

        void Start()
        {

            button = GetComponent<Button>();

            button.onClick.AddListener(OnClickButton);

        }
    }
}
