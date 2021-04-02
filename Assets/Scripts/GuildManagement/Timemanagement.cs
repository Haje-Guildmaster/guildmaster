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
    /// <summary>
    /// 인수만큼의 시간이 지남을 알림.
    /// </summary>
    public event Action<int> TimeChanged;
    
    public Timeblock[] TimeBlockList = new Timeblock[4];
    private TimeTable TimeIndex = TimeTable.Morning;
    private Timeblock CurrentTimeBlock = null; 
    public Timemanagement(Timeblock p1, Timeblock p2, Timeblock p3, Timeblock p4) //좋은 구조는 아닌 것 같음
    {
        TimeBlockList[0] = p1;
        TimeBlockList[1] = p2;
        TimeBlockList[2] = p3;
        TimeBlockList[3] = p4;
        CurrentTimeBlock = TimeBlockList[0];
    }
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
        CurrentTimeBlock = TimeBlockList[(int)TimeIndex];
        Debug.Log("시간대가 변경되었습니다");
        Debug.Log("현재 시각 : " + TimeIndex.ToString());

        TimeChanged?.Invoke(1);
        UiWindowsManager.Instance.ShowMessageBox("시간 변경 알림", "시간대가 변경되었습니다" + "\n현재 시각 : " + TimeIndex.ToString(), new (string, Action)[] { ("확인", () => { }) });
    }
    public void GetEventListNormal()
    {
        foreach (object obj in CurrentTimeBlock.NormalEventList)
        {
            Debug.Log(obj);
        }
    }
    public void GetEventListSpecial()
    {

    }
    public void GetExplorationList()
    {

    }



}
