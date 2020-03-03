using UnityEngine;
using UnityEditor;

public struct CharacterBattleStatData
{
    [SerializeField] private int atk;
    [SerializeField] private int def;
    [SerializeField] private int agi;
    [SerializeField] private int _int;

    private int ATK
    {
        get { return atk; }
        set { atk = value; }
    }
    private int DEF
    {
        get { return def; }
        set { def = value; }
    }
    private int AGI
    {
        get { return agi; }
        set { agi = value; }
    }
    private int INT
    {
        get { return _int; }
        set { _int = value; }
    }
}