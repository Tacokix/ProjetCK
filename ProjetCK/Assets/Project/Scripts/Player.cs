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

    // Event called when the local photon player has been created.
    public static Action<Player> OnLocalPlayerInstantiated;

    private Spell currentSpellUsed;

    #endregion

    #region MonoBehaviour
    protected override void Awake()
    {
        if(!GetComponent<PhotonView>().IsMine)
        {
            Destroy(GetComponent<vThirdPersonController>());
            Destroy(GetComponent<vThirdPersonInput>());
        }
        else
        {
            localInstance = this;
            OnLocalPlayerInstantiated(this);
            OnLocalPlayerInstantiated = null;
        }

        base.Awake();
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
    public override void TryUseSpell(int spellID)
    {
        if(!spellCooldowns.ContainsKey(spellID))
        {
            Debug.Log("Spell not learned");
            return;
        }

        if(spellCooldowns[spellID] > 0)
        {
            // Show 'spell on cooldown' feedback
            Debug.Log("Spell on cooldown.");
            return;
        }

        currentSpellUsed = SpellHandler.GetSpell(spellID);

        if(energy < currentSpellUsed.resourceCost)
        {
            // Show 'not enough resources' feedback
            Debug.Log("Not enough resources.");
            return;
        }

        // try use spell
        if(currentSpellUsed.targetingType == TargetingType.Target)
        {
            // TO DO : Some pathfinding and range check 
            SpellHandler.TryUseSpell(currentSpellUsed, this, currentTarget);
        }
        else if(currentSpellUsed.targetingType == TargetingType.Skillshot)
        {
            // TO DO : Some pathfinding and range check (depending on ground layer)
            SpellHandler.TryUseSpell(currentSpellUsed, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    #endregion
}
