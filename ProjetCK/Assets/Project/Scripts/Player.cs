using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Player : Char
{
    #region Fields
    public static Player localInstance { get; private set; }

    public static Action<Player> OnLocalPlayerInstantiated;

    #endregion

    #region MonoBehaviour
    private void Awake()
    {
        if(!GetComponent<PhotonView>().IsMine)
        {
            Destroy(GetComponent<vThirdPersonController>());
            Destroy(GetComponent<vThirdPersonInput>());
        }
        else
        {
            OnLocalPlayerInstantiated(this);
            OnLocalPlayerInstantiated = null;
        }
    }

    #endregion

    #region Methods
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

    #endregion

    #region Implementation
    public override void UseSpell(int spellID)
    {

    }

    #endregion
}
