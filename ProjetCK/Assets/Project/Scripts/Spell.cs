using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : ScriptableObject
{
    #region Fields
    public int spellID;

    public string spellName;

    public TargetingType targetingType;

    public float resourceCost;

    #endregion

    #region Methods
    public abstract void UseSpell(Char sourceChar, Char target);

    public abstract void UseSpell(Char sourceChar, Vector3 position);

    #endregion
}
