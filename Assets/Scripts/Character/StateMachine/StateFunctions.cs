using System;
using System.Collections;
using UnityEngine;

public static class StateFunctions
{
    public static IEnumerator Recover(CharacterStats stats, CharacterStateMachine stateMachine, double stun)
    {
        double totalStun = stats.CalculateDisadvantage(stun, stateMachine.hitNumber);
        //Debug.Log("Stunlocked for " + totalStun + " ms");
        yield return new WaitForSeconds((float) TimeSpan.FromMilliseconds(totalStun).TotalSeconds);
        stateMachine.TransitionToWalkingOrBlocking();
    }
}
