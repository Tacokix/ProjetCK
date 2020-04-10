using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Char
{
    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void TryApplyDebuff(int debuffID, Char target)
    {
        throw new System.NotImplementedException();
    }

    public override void TryReceiveDebuff(int debuffID)
    {
        throw new System.NotImplementedException();
    }

    public override void UseSpell(int spellID)
    {
        throw new System.NotImplementedException();
    }

}
