using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebuffNames
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

public class DebuffHandler : MonoBehaviour
{
    #region Fields
    /// <summary>
    /// The list of permanent debuffs on the char.
    /// </summary>
    public List<Debuff> debuffs;

    /// <summary>
    /// The list of temporary debuffs applied on the char.
    /// </summary>
    public List<TemporaryDebuff> tempDebuffs;

    #endregion

    #region Methods
    /// <summary>
    /// Adds a temporary debuff if the char does not have that debuff 
    /// or if the debuff's remaining duration is shorter than the new one.
    /// </summary>
    /// <param name="debuffID">The ID of the debuff to add.</param>
    /// <param name="newDuration">The duration of the debuff to add.</param>
    public void AddTempDebuff(int debuffID, float newDuration)
    {
        TemporaryDebuff tempDebuff = GetTempDebuff(debuffID);

        if (tempDebuff != null && tempDebuff.remainingDuration < newDuration)
        {
            tempDebuff.remainingDuration = newDuration;
        }
        else
        {
            tempDebuffs.Add(new TemporaryDebuff(debuffID, newDuration));
        }
    }

    /// <summary>
    /// Removes a temporary debuff on the char.
    /// </summary>
    /// <param name="debuffID">The ID of the debuff to remove.</param>
    public void RemoveTempDebuff(int debuffID)
    {
        TemporaryDebuff tempDebuff = GetTempDebuff(debuffID);

        if (tempDebuff != null)
        {
            StopCoroutine(tempDebuff.debuffCoroutine);
            tempDebuffs.Remove(tempDebuff);
        }
    }

    /// <summary>
    /// Get the temporary debuff on the char associated to an ID.
    /// </summary>
    /// <param name="debuffID">The ID of the debuff to look for on the char.</param>
    /// <returns>The tmeporary debuff if it exists, null if it does not.</returns>
    private TemporaryDebuff GetTempDebuff(int debuffID)
    {
        foreach (TemporaryDebuff tempDebuff in tempDebuffs)
        {
            if (tempDebuff.debuffID == debuffID)
            {
                return tempDebuff;
            }
        }

        return null;
    }

    /// <summary>
    /// Checks if the char has a specific debuff.
    /// </summary>
    /// <param name="debuffID">The ID of the debuff to check.</param>
    /// <returns>True if the char does have the debuff, false otherwise.</returns>
    public bool HasDebuff(int debuffID)
    {
        foreach (Debuff debuff in debuffs)
        {
            if (debuff.debuffID == debuffID)
            {
                return true;
            }
        }

        foreach (Debuff debuff in tempDebuffs)
        {
            if (debuff.debuffID == debuffID)
            {
                return true;
            }
        }

        return false;
    }

    #endregion
}

public class Debuff : MonoBehaviour
{
    public int debuffID;
}

public class TemporaryDebuff : Debuff
{
    /// <summary>
    /// The remaining duration of the debuff.
    /// </summary>
    public float remainingDuration;

    /// <summary>
    /// The coroutine used to control the duration of the debuff.
    /// </summary>
    public Coroutine debuffCoroutine;

    public TemporaryDebuff(int newDebuffID, float duration)
    {
        debuffID = newDebuffID;
        remainingDuration = duration;
        debuffCoroutine = StartCoroutine(DebuffCoroutine());
    }

    /// <summary>
    /// Removes the debuff after a delay.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DebuffCoroutine()
    {
        while (remainingDuration > 0)
        {
            remainingDuration -= Time.deltaTime;
            yield return null;
        }

        // This may lead to errors
        // If it does, then I should link the tempDebuff to the DebuffHandler
        // and call a destruction directly from the DebuffHandler
        Destroy(this);
    }
}
