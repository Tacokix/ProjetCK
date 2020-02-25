using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Char
{
    public override void Die()
    {
        // Do stuff
    }

    public override void TryApplyDebuff(int debuffID, Char target)
    {
        target.TryReceiveDebuff(debuffID);
    }

    public override void TryReceiveDebuff(int debuffID)
    {
        // Check if the debuff can be applied
        // create a DebuffResistance class, and a field in the Char class, that gathers all informations about debuff resistance of the char.

    }
    #region Implementation
    public override void UseSpell(int spellID)
    {

    }

    #endregion
}
