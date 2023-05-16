using System;
using System.Collections;
using UnityEngine;

public static class StateFunctions
{
    public static IEnumerator Recover(CharacterStats stats, CharacterStateMachine stateMachine, double disadvantage)
    {
        double totalDisadvantage = stats.CalculateDisadvantage(disadvantage, stateMachine.hitNumber);
        Debug.Log("Stunlocked for " + totalDisadvantage + " ms");
        yield return new WaitForSeconds((float) TimeSpan.FromMilliseconds(totalDisadvantage).TotalSeconds);
        Debug.Log("Left stunlock");
        stateMachine.TransitionToWalkingOrBlocking();
    }
}
