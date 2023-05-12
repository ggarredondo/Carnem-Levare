using UnityEngine;
using System;

public abstract class Move : ScriptableObject
{
    protected CharacterStats stats;
    protected Character character;
    [SerializeField] private string moveName;

    [Header("Move Animation")]
    [SerializeField] private AnimationClip animation;
    [SerializeField] private float animationSpeed = 1f;

    [Header("Move Sound")]
    [SerializeField] private string initSound;

    [Header("Time Data (ms) (animation events)")]

    [Tooltip("[0, startUp): move is starting.")]
    [SerializeField] private double startUp;

    [Tooltip("[startUp, startUp+active): move is active.")]
    [SerializeField] private double active;

    [Tooltip("[startUp+active, startUp+active+recovery): move is recovering.")]
    [SerializeField] private double recovery;

    [Header("Other Time Data (ms) (animation events)")]

    [Tooltip("Another move can be input buffered at *buffering* ms of performing this move")]
    [SerializeField] private double buffering;

    [Tooltip("Move starts tracking at *startTracking* ms from being performed")]
    [SerializeField] private double startTracking;

    [Tooltip("Move will track for *trackingLength* ms after enabling tracking")]
    [SerializeField] private double trackingLength;

    public virtual void Initialize()
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

    public void AssignEvents()
    {
        #if UNITY_EDITOR
        AnimationEvent initMoveEvent = CreateAnimationEvent("InitMove", 0.0);
        AnimationEvent activateMoveEvent = CreateAnimationEvent("ActivateMove", startUp);
        AnimationEvent deactiveMoveEvent = CreateAnimationEvent("DeactivateMove", startUp + active);
        AnimationEvent endMoveEvent = CreateAnimationEvent("EndMove", startUp + active + recovery);

        AnimationEvent enableBufferingEvent = CreateAnimationEvent("EnableBuffering", buffering);
        AnimationEvent startTrackingEvent = CreateAnimationEvent("StartTracking", startTracking);
        AnimationEvent stopTrackingEvent = CreateAnimationEvent("StopTracking", startTracking + trackingLength);

        AnimationEvent[] events = { initMoveEvent, activateMoveEvent, deactiveMoveEvent, endMoveEvent, 
            enableBufferingEvent, startTrackingEvent, stopTrackingEvent };
        UnityEditor.AnimationUtility.SetAnimationEvents(animation, events);
        #endif
    }

    public abstract void InitMove(in CharacterStats stats);
    public abstract void ActivateMove();
    public abstract void DeactivateMove();
    public abstract void RecoverFromMove();

    public ref readonly AnimationClip Animation { get => ref animation; }
    public ref readonly float AnimationSpeed { get => ref animationSpeed; }
    public ref readonly string InitSound { get => ref initSound; }
}
