using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Char : MonoBehaviour
{
    #region Inspector
    [Header("Settings")]    
    /// <summary>
    /// The current level of the char.
    /// </summary>
    public int level;
    
    /// <summary>
    /// The maximum amount of life.
    /// </summary>
    public float maxLife;
    
    /// <summary>
    /// The type of energy the char uses (Rage, Mana, etc...).
    /// </summary>
    public EnergyType energyType;

    /// <summary>
    /// The current move speed of the char.
    /// </summary>
    public float moveSpeed;

    #endregion

    #region Fields
    /// <summary>
    /// The current amount of life.
    /// </summary>
    [HideInInspector] public float life;


    /// <summary>
    /// The current amount of energy (Rage, Mana, etc...).
    /// </summary>
    [HideInInspector] public float energy;

    /// <summary>
    /// The maximum amount of energy (Rage, Mana, etc...).
    /// </summary>
    [HideInInspector] public float maxEnergy;

    /// <summary>
    /// The current target of the char.
    /// </summary>
    [HideInInspector] public Char target;

    /// <summary>
    /// The debuff handler of the char.
    /// </summary>
    [HideInInspector] public DebuffHandler debuffHandler;

    /// <summary>
    /// The buff handler of the char.
    /// </summary>
    [HideInInspector] public BuffHandler buffHandler;

    #endregion

    #region Methods
    /// <summary>
    /// Cast a spell or an attack.
    /// </summary>
    /// <param name="spellID">The ID of the spell.</param>
    public abstract void UseSpell(int spellID);

    /// <summary>
    /// Performs everything to be done when the char dies.
    /// </summary>
    public abstract void Die();

    /// <summary>
    /// Performs everything to be done when applying a debuff to a target.
    /// </summary>
    /// <param name="debuffID">The ID of the debuff.</param>
    /// <param name="target">The target to debuff.</param>
    public abstract void TryApplyDebuff(int debuffID, Char target);

    // This function will perhaps not be abstract
    public abstract void TryReceiveDebuff(int debuffID);

    /// <summary>
    /// Deals damage to the target.
    /// </summary>
    /// <param name="brutDamage">The damage value dealt to the target before any damage reduction.</param>
    /// <param name="target">The target to deal damage to.</param>
    /// <returns>The net damage dealt.</returns>
    public float DealDamage(float brutDamage, Char target)
    {
        return target.ReceiveDamage(brutDamage);
    }

    /// <summary>
    /// Receives a certain amount of damage, after reduction.
    /// </summary>
    /// <param name="brutDamage">The brut damage received.</param>
    /// <returns>The net damage received.</returns>
    public float ReceiveDamage(float brutDamage)
    {
        float netDamage = brutDamage;

        // To implement : damage reduction

        // if the target is dead
        if(Mathf.Clamp(life - netDamage, 0, maxLife) == 0)
        {
            netDamage = life;
            life = 0;
            Die();
        }

        return netDamage;
    }

    #endregion
}

public enum EnergyType
{
    Rage,
    Mana
}