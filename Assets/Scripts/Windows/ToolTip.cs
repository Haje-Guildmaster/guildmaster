using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GuildMaster.Windows;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]public string Tooltipname = null;
    [TextArea]public string Tooltipstring = null;
    void Awake()
    {
        // = GetComponent<Text>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        StatusToolTip.Instance.showToolTip(Tooltipname,Tooltipstring);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        StatusToolTip.Instance.hideToolTip();
    }
}
