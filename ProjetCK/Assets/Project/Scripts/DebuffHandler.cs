using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebuffHandler
{
    // The list of Debuffs
    public const int Debuff1 = 1;
    public const int Debuff2 = 2;
    public const int Debuff3 = 3;
    public const int Debuff4 = 4;
    public const int Debuff5 = 5;
    public const int Debuff6 = 6;
    public const int Debuff7 = 7;
    public const int Debuff8 = 8;
}

public class DebuffState
{
    #region Fields
    public List<Debuff> debuffs;

    #endregion
}

public class Debuff
{
    /// <summary>
    /// The type of the debuff.
    /// </summary>
    public int debuffID;

    /// <summary>
    /// The remaining duration of the debuff.
    /// </summary>
    public float duration;

    public Debuff(int _debuffID, float _duration)
    {
        debuffID = _debuffID;
        duration = _duration;
    }
}
