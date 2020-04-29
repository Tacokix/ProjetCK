using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;

public abstract class Char : MonoBehaviourPunCallbacks
{
    #region Inspector
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

    /// <summary>
    /// The set of spells known by the player.
    /// </summary>
    public List<Spell> knownSpells;

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
    [HideInInspector] public Char currentTarget;

    /// <summary>
    /// The debuff handler of the char.
    /// </summary>
    [HideInInspector] public DebuffHandler debuffHandler;

    /// <summary>
    /// The buff handler of the char.
    /// </summary>
    [HideInInspector] public BuffHandler buffHandler;

    /// <summary>
    /// Gather all remaining cooldown of available spells.
    /// </summary>
    protected Dictionary<int, float> spellCooldowns;

    /// <summary>
    /// Event called when the char deals damage.
    /// Usefull for effects like lifesteal, etc..
    /// </summary>
    protected Action<float> OnDamageDealt;

    #endregion

    #region MonoBehaviour
    protected virtual void Awake()
    {
        life = maxLife;

        // Gather all known spells
        spellCooldowns = new Dictionary<int, float>();

        foreach (Spell knownSpell in knownSpells)
        {
            foreach (Spell spellData in SpellHandler.spells)
            {
                if (knownSpell.spellID == spellData.spellID)
                {
                    spellCooldowns.Add(knownSpell.spellID, 0f);
                }
            }
        }
    }

    private void OnMouseDown()
    {
        // The local player will target this instance of char
        Player.localInstance.Target(this);
    }

    private List<int> _spellKeys = new List<int>();

    private void Update()
    {
        // Update all cooldowns
        // TO DO : Optimize by storing the keys in a cache, only updated when the known spells are updated
        _spellKeys.Clear();
        foreach(int id in spellCooldowns.Keys)
        {
            _spellKeys.Add(id);
        }

        foreach(int id in _spellKeys)
        {
            spellCooldowns[id] = Mathf.Max(0, spellCooldowns[id] - Time.deltaTime);
        }
    }

    //private void OnMouseEnter()
    //{
    //    Debug.Log("OnMouseEnter");
    //    Highlight(transform, true);   
    //}

    //private void OnMouseExit()
    //{
    //    Debug.Log("OnMouseExit");
    //    Highlight(transform, false);
    //}

    #endregion

    #region Methods
    /// <summary>
    /// Cast a spell or an attack.
    /// </summary>
    /// <param name="spellID">The ID of the spell.</param>
    public abstract void TryUseSpell(int spellID);

    /// <summary>
    /// Call the die function of the killed target through the server.
    /// </summary>
    /// <param name="target">The target killed.</param>
    public void KillTarget(Char target)
    {
        target.photonView.RPC("Die", RpcTarget.AllViaServer);
    }

    /// <summary>
    /// Performs everything to be done when the char dies.
    /// </summary>
    [PunRPC]
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
        Debug.LogError($"Deal {brutDamage} Brut damage");

        float netDamage = brutDamage;

        //if (Mathf.Clamp(target.life - netDamage, 0, target.maxLife) == 0)
        //{
        //    netDamage = target.life;
        //    target.life = 0;
        //    KillTarget(target);
        //}

        // To implement : damage reduction
        if(target.photonView.IsMine)
        {
            ReceiveDamage(PhotonNetwork.LocalPlayer.ActorNumber, netDamage);
        }
        else
        {
            target.photonView.RPC("ReceiveDamage", target.photonView.Owner, PhotonNetwork.LocalPlayer.ActorNumber, netDamage);
        }

        //target.photonView.RPC("ReceiveDamage", RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer.ActorNumber, netDamage);
        return netDamage;
    }

    /// <summary>
    /// Receives a certain amount of damage, after reduction.
    /// </summary>
    /// <param name="sourcePlayer">The player that dealt the damages.</param>
    /// <param name="brutDamage">The brut damage received.</param>
    /// <returns>The net damage received.</returns>
    [PunRPC]
    public void ReceiveDamage(int sourcePlayer, float brutDamage)
    {
        Debug.Log($"Receive {brutDamage} Brut damage from {sourcePlayer} player");

        float netDamage = brutDamage;

        // To implement : damage reduction

        // if the target is dead
        if(netDamage >= life)
        {
            netDamage = life;
            UpdateLife(0);
        }
        else
        {
            UpdateLife(life - netDamage);
        }

        photonView.RPC("DamageReceived", RpcTarget.Others, sourcePlayer, netDamage);
    }

    /// <summary>
    /// Informs the remote players that the char has received damages.
    /// </summary>
    /// <param name="sourcePlayer">The player that dealt the damages.</param>
    /// <param name="netDamage">The net damages received.</param>
    [PunRPC]
    protected void DamageReceived(int sourcePlayer, float netDamage)
    {
        // If the player who dealt the damages is the local player, do stuff
        if(sourcePlayer == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            OnDamageDealt?.Invoke(netDamage);
        }

        // Updates the local value of the char's health
        UpdateLife(life - netDamage);
    }

    /// <summary>
    /// Updates the current life and the UI.
    /// </summary>
    /// <param name="newValue">The new health value.</param>
    protected void UpdateLife(float newValue)
    {
        life = newValue;

        if(life == 0 && photonView.IsMine)
        {
            Die();
        }

        // TO DO : Update life UI
    }

    #endregion

    #region Targeting Methods
    /// <summary>
    /// Targets a char.
    /// </summary>
    /// <param name="target">The char to target.</param>
    public void Target(Char target)
    {
        // Stop targeting the current target
        if(currentTarget != null)
        {
            currentTarget.Highlight(false);
        }

        if(target != null)
        {
            currentTarget = target;
            target.Highlight(true);

            // When the local player targets an enemy, show the health bar
            if(this == Player.localInstance)
            {
                HealthBarUI.Instance.ShowUI(target);
            }
        }
    }

    /// <summary>
    /// Highlights the char by changing its layer.
    /// </summary>
    /// <param name="active">If true, activate the highlight.</param>
    private void Highlight(bool active)
    {
        Highlight(transform, active);
    }

    /// <summary>
    /// Recurively performs the highlight of the current Char.
    /// </summary>
    /// <param name="active">If true, activate the highlight.</param>
    private void Highlight(Transform transf, bool active)
    {
        foreach(Transform t in transf)
        {
            Highlight(t, active);
        }

        transf.gameObject.layer = LayerMask.NameToLayer(active ? "Outline" : "Default");
    }

    #endregion
}

public enum EnergyType
{
    None,
    Mana,
    Rage
}