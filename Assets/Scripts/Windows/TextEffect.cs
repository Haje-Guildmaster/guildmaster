using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class TextEffect : MonoBehaviour
{
    string targetMsg;
    public int CharPerSeconds;
    Text msgText;
    private int index;
    private float interval;
    public bool isAnim;
    private void Awake()
    {
        msgText = GetComponent<Text>();
    }
    public void SetMsg(string msg)
    {
        if (isAnim)
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
    void EffectStart()
    {
        msgText.text = "";
        index = 0;
        interval = 1.0f / CharPerSeconds;
        isAnim = true;
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
        isAnim = false;
        return;
    }
}
