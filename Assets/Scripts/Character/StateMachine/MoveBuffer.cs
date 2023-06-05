using System.Collections;
using System;
using UnityEngine;

public class MoveBuffer
{
    private readonly CharacterStateMachine stateMachine;
    private readonly CharacterStats stats;
    private Action nextTransition;

    private WaitForSeconds waitForSeconds;
    private IEnumerator coroutine;
    private bool BUFFER_FLAG = false;

    public MoveBuffer(double msBeforeBuffering, in CharacterStateMachine stateMachine, in CharacterStats stats)
    {
        waitForSeconds = new WaitForSeconds((float)TimeSpan.FromMilliseconds(msBeforeBuffering).TotalSeconds);
        this.stateMachine = stateMachine;
        this.stats = stats;
    }

    public void ListenTo(ref Action<int> OnEvent)
    {
        OnEvent += BufferMove;
        nextTransition = stateMachine.TransitionToWalkingOrBlocking;
        coroutine = AllowBuffering();
        stateMachine.StartCoroutine(coroutine);
    }
    public void StopListening(ref Action<int> OnEvent)
    {
        OnEvent -= BufferMove;
        stateMachine.StopCoroutine(coroutine);
    }
    public void NextTransition() => nextTransition.Invoke();

    private IEnumerator AllowBuffering()
    {
        yield return waitForSeconds;
        BUFFER_FLAG = true;
    }

    private void BufferMove(int moveIndex) 
    {
        if (BUFFER_FLAG && moveIndex >= 0 && moveIndex < stats.MoveList.Count)
        {
            BUFFER_FLAG = false;
            nextTransition = () => stateMachine.TransitionToMove(moveIndex);
        }
    }
}
