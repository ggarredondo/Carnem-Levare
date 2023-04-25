using UnityEngine;
using System;

public abstract class Move : ScriptableObject
{
    [SerializeField] private string moveName;

    [Header("Move Animation")]
    [SerializeField] private AnimationClip animation;
    [SerializeField] private float animationSpeed = 1f;

    [Header("Time Data (ms)")]
    [SerializeField] private double startUp;
    [SerializeField] private double active;
    [SerializeField] private double recovery;

    private void Awake()
    {
        if (animation != null) AssignEvents();
    }

    private AnimationEvent CreateAnimationEvent(string functionName, double timeInMs)
    {
        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.functionName = functionName;
        animationEvent.time = (float)TimeSpan.FromMilliseconds(timeInMs).TotalSeconds;
        return animationEvent;
    }

    private void AssignEvents()
    {
        #if UNITY_EDITOR
        AnimationEvent initMoveEvent = CreateAnimationEvent("InitMove", 0.0);
        AnimationEvent activateMoveEvent = CreateAnimationEvent("ActivateMove", startUp);
        AnimationEvent deactiveMoveEvent = CreateAnimationEvent("DeactivateMove", startUp + active);
        AnimationEvent recoverFromMoveEvent = CreateAnimationEvent("RecoverFromMove", startUp + active + recovery);

        AnimationEvent[] events = { initMoveEvent, activateMoveEvent, deactiveMoveEvent, recoverFromMoveEvent };
        UnityEditor.AnimationUtility.SetAnimationEvents(animation, events);
        #endif
    }

    public abstract void InitMove();
    public abstract void ActivateMove();
    public abstract void DeactivateMove();
    public abstract void RecoverFromMove();
    public ref readonly float AnimationSpeed { get => ref animationSpeed; }
}
