using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuildMaster.Data;

public class TimeButton : MonoBehaviour
{
    public void NextTime()
    {
        Player.Instance.TimeManager.GotoNextTime();
    }

}
