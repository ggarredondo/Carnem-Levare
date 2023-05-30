using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class Move : ScriptableObject
{
    protected List<string> stringData;
    protected CharacterStats stats;
    protected Character character;

    [SerializeField] protected Sprite icon;
    [SerializeField] protected string moveName;

    [Header("Move Animation")]
    [SerializeField] protected AnimationClip animation;
    [SerializeField] protected float animationSpeed = 1f;

    [Tooltip("Will reset animators if in play mode")]
    [SerializeField] protected bool applyAnimationEvents = false;

    [Header("Move Sound")]
    [SerializeField] protected string initSound;

    [Header("Time Data (ms) (animation events)")]

    [Tooltip("[0, startUp): move is starting.")]
    [SerializeField] protected double startUp;

    [Tooltip("[startUp, startUp+active): move is active.")]
    [SerializeField] protected double active;

    [Tooltip("[startUp+active, startUp+active+recovery): move is recovering.")]
    [SerializeField] protected double recovery;

    [Header("Other Time Data (ms) (animation events)")]

    [Tooltip("Move starts tracking at *startTracking* ms from being performed")]
    [SerializeField] protected double startTracking;

    [Tooltip("Move will track for *trackingLength* ms after enabling tracking")]
    [SerializeField] protected double trackingLength;

    protected abstract void UpdateStringData();
    private void OnEnable()
    {
        stringData = new List<string>();
        UpdateStringData();
    }

    private void OnValidate()
    {
        if (applyAnimationEvents)
        {
            AssignEvents();
            applyAnimationEvents = false;
        }
        UpdateStringData();
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

        AnimationEvent startTrackingEvent = CreateAnimationEvent("StartTracking", startTracking);
        AnimationEvent stopTrackingEvent = CreateAnimationEvent("StopTracking", startTracking + trackingLength);

        AnimationEvent[] events = { initMoveEvent, activateMoveEvent, deactiveMoveEvent, endMoveEvent, startTrackingEvent, stopTrackingEvent };
        UnityEditor.AnimationUtility.SetAnimationEvents(animation, events);
        #endif
    }

    public abstract void InitMove(in CharacterStats stats);
    public abstract void ActivateMove();
    public abstract void DeactivateMove();
    public abstract void EndMove();

    public ref readonly AnimationClip Animation { get => ref animation; }
    public ref readonly float AnimationSpeed { get => ref animationSpeed; }
    public ref readonly string InitSound { get => ref initSound; }

    public double StartUp => startUp;
    public double Active => active;
    public double Recovery => recovery;

    public double RelativeStartUp => startUp / animationSpeed;
    public double RelativeActive => active / animationSpeed;
    public double RelativeRecovery => recovery / animationSpeed;

    public ref readonly List<string> StringData => ref stringData;
}
