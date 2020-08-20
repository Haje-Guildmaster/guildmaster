using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GuildMaster.GuildManagement;

enum Range
{
    Min = -100,
    Max = 100,
    Threshold = 0
}
public class ReputationManager // use it in Guild class
{
    private int reputation = 0;
    public void ChangeReputation(int given, Guild guildobj)
    {
        guildobj.Reputation.Value = given;
    }
    public void AddReputation(int given, Guild guildobj)
    {
        guildobj.Reputation.Value += given;
    }
    public int Reputation
    {
        get { return reputation; }
        set { reputation = value; }
    }
    public bool IsRepPositive(Guild guildobj)
    {
        return guildobj.Reputation >= 0;
    }
    public float GetRepPercentage(Guild guildobj)
    {
        return (float)guildobj.Reputation / (float)Range.Max; //casting 제대로 되는 것 맞나?
    }

}
