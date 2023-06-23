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

    [Header("Time Data (ms) (animation events)")]

    [Tooltip("[0, startUp): move is starting.")]
    [SerializeField] protected double startUp;

    [Tooltip("[startUp, startUp+active): move is active.")]
    [SerializeField] protected double active;

    [Tooltip("[startUp+active, startUp+active+recovery): move is recovering.")]
    [SerializeField] protected double recovery;

    public bool TRACKING_FLAG { get; protected set; }

    protected abstract void UpdateStringData();
    private void OnEnable()
    {
        stringData = new List<string>();
        UpdateStringData();
        TRACKING_FLAG = false;
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

        AnimationEvent[] events = { initMoveEvent, activateMoveEvent, deactiveMoveEvent, endMoveEvent };
        UnityEditor.AnimationUtility.SetAnimationEvents(animations[0].animation, events);

        // Make sure the remaining animations have no events
        AnimationEvent[] empty = {};
        for (int i = 1; i < animations.Count; ++i) {
            if (animations[i].animation != animations[0].animation)
                UnityEditor.AnimationUtility.SetAnimationEvents(animations[i].animation, empty);
        }
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
        animatorScriptController.AddAnyStateTransition(0, moveName, conditions, true, transitionDuration, TransitionInterruptionSource.Source);
    }

    public abstract void InitMove(in CharacterStats stats);
    public abstract void ActivateMove(in CharacterStats stats);
    public abstract void DeactivateMove(in CharacterStats stats);
    public abstract void EndMove(in CharacterStats stats);

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
}
