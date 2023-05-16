using System;
using System.Collections;
using UnityEngine;

public static class StateFunctions
{
    public static IEnumerator Recover(CharacterStats stats, CharacterStateMachine stateMachine, double disadvantage)
    {
        double totalDisadvantage = stats.CalculateDisadvantage(disadvantage, stateMachine.hitNumber);
        yield return new WaitForSeconds((float) TimeSpan.FromMilliseconds(totalDisadvantage).TotalSeconds);
        stateMachine.TransitionToWalkingOrBlocking();
    }
}
