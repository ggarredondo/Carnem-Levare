using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

public abstract class Move : ScriptableObject
{
    protected List<string> stringData;
    protected CharacterStats stats;
    protected Character character;

    [SerializeField] protected Sprite icon;
    [SerializeField] protected string moveName;

    [Header("Animation")]
    [SerializeField] protected string animatorTrigger;
    [SerializeField] protected List<BlendTree2DMotion> animations;
    [SerializeField] protected float stateSpeed = 1f;
    [SerializeField] protected float directionSpeed = 1f;

    [Tooltip("Will reset animators if in play mode")]
    [SerializeField] protected bool applyAnimationEvents = false;

    [Header("Animator")]
    [SerializeField] protected AnimatorController animatorController;
    [SerializeField] protected float transitionDuration = 0.1f;

    [Tooltip("Will reset animators if in play mode")]
    [SerializeField] protected bool applyAnimationState = false;

    [Header("Move Sound")]
    [SerializeField] protected string initSound;

    [Header("Flags")]
    [SerializeField] protected bool hyperarmor;

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
            applyAnimationEvents = false;
            AssignEvents();
        }

        if (applyAnimationState)
        {
            applyAnimationState = false;
            AddState();
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
        UnityEditor.AnimationUtility.SetAnimationEvents(animations[0].animation, events);
        #endif
    }

    public void AddState()
    {
        AnimatorScriptController animatorScriptController = new AnimatorScriptController(animatorController);
        animatorScriptController.AddBlendTreeState(moveName, stateSpeed, animations, "horizontal", "vertical", BlendTreeType.FreeformDirectional2D);
        animatorScriptController.AddParameter(animatorTrigger, AnimatorControllerParameterType.Trigger);

        AnimatorCondition[] conditions = new AnimatorCondition[1];
        conditions[0].mode = AnimatorConditionMode.If;
        conditions[0].parameter = animatorTrigger;
        conditions[0].threshold = 0f;
        animatorScriptController.AddAnyStateTransition(moveName, conditions, true, transitionDuration, TransitionInterruptionSource.Source);
    }

    public abstract void InitMove(in CharacterStats stats);
    public abstract void ActivateMove();
    public abstract void DeactivateMove();
    public abstract void EndMove();

    public ref readonly Sprite Icon => ref icon;
    public ref readonly string AnimatorTrigger => ref animatorTrigger;
    public ref readonly float DirectionSpeed => ref directionSpeed;
    public ref readonly string InitSound { get => ref initSound; }

    public double StartUp => startUp;
    public double Active => active;
    public double Recovery => recovery;

    public double RelativeStartUp => startUp / stateSpeed;
    public double RelativeActive => active / stateSpeed;
    public double RelativeRecovery => recovery / stateSpeed;

    public ref readonly List<string> StringData => ref stringData;

    public ref readonly bool Hyperarmor => ref hyperarmor;
}
