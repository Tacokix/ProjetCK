using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BuffNames
{
    // The list of Buffs
    public const int Buff1 = 1;
    public const int Buff2 = 2;
    public const int Buff3 = 3;
    public const int Buff4 = 4;
    public const int Buff5 = 5;
    public const int Buff6 = 6;
    public const int Buff7 = 7;
    public const int Buff8 = 8;
}

public class BuffHandler : MonoBehaviour
{
    #region Fields
    /// <summary>
    /// The list of permanent buffs on the char.
    /// </summary>
    public List<Buff> buffs;

    /// <summary>
    /// The list of temporary buffs applied on the char.
    /// </summary>
    public List<TemporaryBuff> tempBuffs;

    #endregion

    #region Methods
    /// <summary>
    /// Adds a temporary buff if the char does not have that buff 
    /// or if the buff's remaining duration is shorter than the new one.
    /// </summary>
    /// <param name="buffID">The ID of the buff to add.</param>
    /// <param name="newDuration">The duration of the buff to add.</param>
    public void AddTempBuff(int buffID, float newDuration)
    {
        TemporaryBuff tempBuff = GetTempBuff(buffID);

        if (tempBuff != null && tempBuff.remainingDuration < newDuration)
        {
            tempBuff.remainingDuration = newDuration;
        }
        else
        {
            tempBuffs.Add(new TemporaryBuff(buffID, newDuration));
        }
    }

    /// <summary>
    /// Removes a temporary buff on the char.
    /// </summary>
    /// <param name="buffID">The ID of the buff to remove.</param>
    public void RemoveTempBuff(int buffID)
    {
        TemporaryBuff tempBuff = GetTempBuff(buffID);

        if(tempBuff != null)
        {
            StopCoroutine(tempBuff.buffCoroutine);
            tempBuffs.Remove(tempBuff);
        }
    }

    /// <summary>
    /// Get the temporary buff on the char associated to an ID.
    /// </summary>
    /// <param name="buffID">The ID of the buff to look for on the char.</param>
    /// <returns>The tmeporary buff if it exists, null if it does not.</returns>
    private TemporaryBuff GetTempBuff(int buffID)
    {
        foreach(TemporaryBuff tempBuff in tempBuffs)
        {
            if(tempBuff.buffID == buffID)
            {
                return tempBuff;
            }
        }

        return null;
    }

    /// <summary>
    /// Checks if the char has a specific buff.
    /// </summary>
    /// <param name="buffID">The ID of the buff to check.</param>
    /// <returns>True if the char does have the buff, false otherwise.</returns>
    public bool HasBuff(int buffID)
    {
        foreach(Buff buff in buffs)
        {
            if(buff.buffID == buffID)
            {
                return true;
            }
        }

        foreach(Buff buff in tempBuffs)
        {
            if(buff.buffID == buffID)
            {
                return true;
            }
        }

        return false;
    }

    #endregion
}

public class Buff : MonoBehaviour
{
    public int buffID;
}

public class TemporaryBuff : Buff
{
    /// <summary>
    /// The remaining duration of the buff.
    /// </summary>
    public float remainingDuration;

    /// <summary>
    /// The coroutine used to control the duration of the buff.
    /// </summary>
    public Coroutine buffCoroutine;

    public TemporaryBuff(int newBuffID, float duration)
    {
        buffID = newBuffID;
        remainingDuration = duration;
        buffCoroutine = StartCoroutine(BuffCoroutine());
    }
    
    /// <summary>
    /// Removes the buff after a delay.
    /// </summary>
    /// <returns></returns>
    private IEnumerator BuffCoroutine()
    {
        while(remainingDuration > 0)
        {
            remainingDuration -= Time.deltaTime;
            yield return null;
        }

        // This may lead to errors
        // If it does, then I should link the tempBuff to the buffHandler
        // and call a destruction directly from the buffHandler
        Destroy(this);
    }
}
