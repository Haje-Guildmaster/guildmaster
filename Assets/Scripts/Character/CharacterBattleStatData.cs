using UnityEngine;
using UnityEditor;

public struct CharacterBattleStatData
{
    [SerializeField] private int atk;
    [SerializeField] private int def;
    [SerializeField] private int agi;
    [SerializeField] private int _int;

    public int ATK
    {
        get { return atk; }
        set { atk = value; }
    }
    public int DEF
    {
        get { return def; }
        set { def = value; }
    }
    public int AGI
    {
        get { return agi; }
        set { agi = value; }
    }
    public int INT
    {
        get { return _int; }
        set { _int = value; }
    }
}