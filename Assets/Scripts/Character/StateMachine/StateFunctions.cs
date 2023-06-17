using System;
using System.Collections;
using UnityEngine;

public static class StateFunctions
{
    public static IEnumerator Recover(CharacterStateMachine stateMachine, double stun)
    {
        //Debug.Log("Stunlocked for " + stun + " ms");
        yield return new WaitForSeconds((float) TimeSpan.FromMilliseconds(stun).TotalSeconds);
        //Debug.Log("Left stunlock");
        stateMachine.TransitionToWalkingOrBlocking();
    }
}
