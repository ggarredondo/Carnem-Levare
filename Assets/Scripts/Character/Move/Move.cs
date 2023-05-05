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

    [Header("Time Data (ms)")]
    [SerializeField] private double startUp;
    [SerializeField] private double active;
    [SerializeField] private double recovery;
    [SerializeField] private double buffering;

    public virtual void Initialize(in Character character, in CharacterStats stats)
    {
        this.character = character;
        this.stats = stats;
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
        AnimationEvent enableBufferingEvent = CreateAnimationEvent("EnableBuffering", buffering);

        AnimationEvent[] events = { initMoveEvent, activateMoveEvent, deactiveMoveEvent, recoverFromMoveEvent, enableBufferingEvent };
        UnityEditor.AnimationUtility.SetAnimationEvents(animation, events);
        #endif
    }

    public abstract void InitMove();
    public abstract void ActivateMove();
    public abstract void DeactivateMove();
    public abstract void RecoverFromMove();

    public ref readonly AnimationClip Animation { get => ref animation; }
    public ref readonly float AnimationSpeed { get => ref animationSpeed; }
    public ref readonly string InitSound { get => ref initSound; }
}
