using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GuildMaster.Windows;

enum TimeTable
{
    Morning = 0,
    Afternoon = 1,
    Night = 2,
    Dawn = 3
}
public class Timemanagement 
{
    private TimeTable TimeIndex = TimeTable.Morning;

    public void GotoNextTime()
    {
        if (TimeIndex == TimeTable.Dawn)
        {
            TimeIndex = TimeTable.Morning;
        }
        else
        {
            TimeIndex++;
        }
        Debug.Log("시간대가 변경되었습니다");
        Debug.Log("현재 시각 : " + TimeIndex.ToString());
        UiWindowsManager.Instance.ShowMessageBox("시간 변경 알림", "시간대가 변경되었습니다" + "\n현재 시각 : " + TimeIndex.ToString(), new (string, Action)[] { ("확인", () => { }) });
    }
}
