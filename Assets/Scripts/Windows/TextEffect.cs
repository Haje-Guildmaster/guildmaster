using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GuildMaster.Windows;
[RequireComponent(typeof(Text))]
public class TextEffect : MonoBehaviour
{
    string targetMsg;
    Text msgText;
    private int index;
    private float interval;
    private void Awake()
    {
        msgText = GetComponent<Text>();
    }
    public void SetMsg(string msg)
    {
        if (SettingVariables.isAnim)
        {
            msgText.text = targetMsg;
            CancelInvoke();
            EffectEnd();
        }
        else
        {
            targetMsg = msg;
            EffectStart();
        }
    }
    public void SetCharPerSeconds(float _index)
    {
        int[] _speeds = new int[4] {10, 25, 50, 1000};
        SettingVariables.CharPerSeconds = _speeds[(int)_index];
    }
    void EffectStart()
    {
        msgText.text = "";
        index = 0;
        interval = 1.0f / SettingVariables.CharPerSeconds;
        SettingVariables.isAnim = true;
        Invoke("EffectOn", interval);
    }
    void EffectOn()
    {
        if(msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }
        msgText.text += targetMsg[index];
        index++;

        Invoke("EffectOn", interval);
    }
    void EffectEnd()
    {
        SettingVariables.isAnim = false;
        return;
    }
}
